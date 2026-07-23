using OrderGenerator.Application.Contracts;
using OrderGenerator.Application.DTOs;
using OrderGenerator.Domain.Models;
using QuickFix;
using QuickFix.Fields;
using QuickFix.FIX44;

namespace OrderGenerator.Infrastructure.Fix;

public class FixOrderAccumulatorClient(PendingExecutionReportStore pendingStore) : IOrderAccumulatorClient
{
    private readonly SessionID _sessionId = new(
            "FIX.4.4",
            "ORDER_GENERATOR",
            "ORDER_ACCUMULATOR");

    public async Task<ExecutionReportDto> SendAsync(Order order, CancellationToken cancellationToken)
    {
        var pending =pendingStore.Register(order.Id, cancellationToken);

        var message = new NewOrderSingle();

        message.SetField(
            new ClOrdID(order.Id.ToString()));

        message.SetField(
            new Symbol(order.Symbol.Value));

        message.SetField(
            new QuickFix.Fields.Side(
                order.Side == Domain.Models.Side.Buy
                    ? QuickFix.Fields.Side.BUY
                    : QuickFix.Fields.Side.SELL));

        message.SetField(
            new OrderQty(order.Quantity));

        message.SetField(
            new Price(order.Price));

        message.SetField(
            new OrdType(
                OrdType.LIMIT));

        message.SetField(
            new TransactTime(DateTime.UtcNow));

        try
        {
            Session.SendToTarget(message, _sessionId);
        }
        catch(Exception ex)
        {
            pendingStore.Fail(order.Id, ex);
            throw;
        }

        var fixReport = await pending;

        return new ExecutionReportDto(
            MapExecType(fixReport.ExecType.Value),
            order.Symbol,
            MapSide(fixReport.Side.Value),
            (int)fixReport.OrderQty.Value,
            order.Price,
            fixReport.IsSetText() ? fixReport.Text.Value : null);
    }

    private static Domain.Models.ExecType MapExecType(char value)
    {
        return value switch
        {
            QuickFix.Fields.ExecType.NEW => Domain.Models.ExecType.New,
            QuickFix.Fields.ExecType.REJECTED => Domain.Models.ExecType.Rejected,
            _ => throw new NotSupportedException($"ExecType {value} not supported")
        };
    }

    private static Domain.Models.Side MapSide(char value)
    {
        return value switch
        {
            QuickFix.Fields.Side.BUY => Domain.Models.Side.Buy,
            QuickFix.Fields.Side.SELL => Domain.Models.Side.Sell,
            _ => throw new NotSupportedException($"Side {value} not supported")
        };
    }
}
