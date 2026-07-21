using MediatR;
using OrderAccumulator.Application.Commands;
using OrderAccumulator.Application.Contracts;
using OrderAccumulator.Application.DTOs;
using OrderAccumulator.Domain.Exceptions;
using OrderAccumulator.Domain.Models;
using OrderAccumulator.Domain.ValueObjects;

namespace OrderAccumulator.Application.Handlers;

public class ProcessOrderCommandHandler(
    IOrderRepository _orderRepository,
    IExposureRepository _exposureRepository,
    IUnitOfWork _unitOfWork) : IRequestHandler<ProcessOrderCommand, ProcessOrderResultDto>
{
    public async Task<ProcessOrderResultDto> Handle(ProcessOrderCommand request, CancellationToken cancellationToken)
    {
        var exposure = await GetOrCreateExposureAsync(request.Symbol, cancellationToken);
        var order = CreateNewOrder(request);

        try
        {
            exposure.AddOrder(order);
            
            await AddOrderAsync(order, cancellationToken);
            await AddOrUpdateExposureAsync(exposure, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            
            return CreateSuccessResult(request, exposure);
        }
        catch (DomainException ex)
        {
            var rejectedOrder = CreateRejectedOrder(request);

            await _orderRepository.AddAsync(rejectedOrder, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return CreateRejectedResult(request, exposure, ex.Message);
        }
    }

    private static Order CreateNewOrder(ProcessOrderCommand request) =>
        Order.Create(request.Symbol, request.Side, request.Quantity, request.Price, ExecType.New);

    private static Order CreateRejectedOrder(ProcessOrderCommand request) =>
        Order.Create(request.Symbol, request.Side, request.Quantity, request.Price, ExecType.Rejected);

    private async Task<Exposure> GetOrCreateExposureAsync(Symbol symbol, CancellationToken cancellationToken)
    {
        var exposure = await _exposureRepository.GetBySymbolAsync(symbol, cancellationToken);
        return exposure ?? Exposure.Create(symbol);
    }

    private async Task AddOrderAsync(Order order, CancellationToken cancellationToken)
    {
        await _orderRepository.AddAsync(order, cancellationToken);
    }

    private async Task AddOrUpdateExposureAsync(Exposure exposure, CancellationToken cancellationToken)
    {
        var existingExposure = await _exposureRepository.GetBySymbolAsync(exposure.Symbol, cancellationToken);

        if (existingExposure is null)
        {
            await _exposureRepository.AddAsync(exposure, cancellationToken);
            return;
        }

        await _exposureRepository.UpdateAsync(exposure, cancellationToken);
    }

    private static ProcessOrderResultDto CreateSuccessResult(ProcessOrderCommand request, Exposure exposure) =>
        new(
            ExecType.New,
            request.Symbol,
            request.Side,
            request.Quantity,
            request.Price,
            exposure.CurrentExposure,
            null);

    private static ProcessOrderResultDto CreateRejectedResult(ProcessOrderCommand request, Exposure exposure, string errorMessage) =>
        new(
            ExecType.Rejected,
            request.Symbol,
            request.Side,
            request.Quantity,
            request.Price,
            exposure.CurrentExposure,
            errorMessage);
}
