using Microsoft.Extensions.DependencyInjection;
using OrderAccumulator.Application.Contracts;
using QuickFix;
using QuickFix.Fields;

namespace OrderAccumulator.Infrastructure.Fix;

public class FixApplication(IServiceScopeFactory scopeFactory) : IApplication
{
    public void OnCreate(SessionID sessionID)
    {
        Console.WriteLine($"FIX Session created: {sessionID}");
    }

    public void OnLogon(SessionID sessionID)
    {
        Console.WriteLine($"FIX Logon: {sessionID}");
    }

    public void OnLogout(SessionID sessionID)
    {
        Console.WriteLine($"FIX Logout: {sessionID}");
    }

    public void FromAdmin(Message message, SessionID sessionID)
    {
    }

    public void ToAdmin(Message message, SessionID sessionID)
    {
    }

    public void FromApp(Message message, SessionID sessionID)
    {
        Console.WriteLine($"Received FIX message: {message}");

        var msgType = new MsgType();
        message.Header.GetField(msgType);

        if (msgType.Value != MsgType.ORDER_SINGLE)
        {
            return;
        }

        _ = ProcessOrderAsync(message, sessionID);
    }

    private async Task ProcessOrderAsync(Message message, SessionID sessionID)
    {
        try
        {
            var scope = scopeFactory.CreateScope();

            var orderProcessor = scope.ServiceProvider.GetRequiredService<IOrderProcessor>();

            var order = (QuickFix.FIX44.NewOrderSingle)message;

            var result = await orderProcessor.ProcessAsync(
                Domain.ValueObjects.Symbol.Create(order.Symbol.Value),
                order.Side.Value == Side.BUY
                    ? Domain.Models.Side.Buy
                    : Domain.Models.Side.Sell,
                (int)order.OrderQty.Value,
                order.Price.Value,
                CancellationToken.None);

            var executionReport = new QuickFix.FIX44.ExecutionReport();

            executionReport.SetField(
                new OrderID(Guid.NewGuid().ToString()));

            executionReport.SetField(
                new ExecID(Guid.NewGuid().ToString()));

            executionReport.SetField(
                new ExecType(
                    result.ExecType == Domain.Models.ExecType.New
                        ? ExecType.NEW
                        : ExecType.REJECTED));

            executionReport.SetField(
                new OrdStatus(
                    result.ExecType == Domain.Models.ExecType.New
                        ? OrdStatus.NEW
                        : OrdStatus.REJECTED));

            executionReport.SetField(
                new ClOrdID(order.ClOrdID.Value));

            executionReport.SetField(
                new Symbol(result.Symbol.Value));

            executionReport.SetField(
                new Side(
                    result.Side == Domain.Models.Side.Buy
                        ? Side.BUY
                        : Side.SELL));

            executionReport.SetField(
                new OrderQty(result.Quantity));

            executionReport.SetField(
                new LeavesQty(result.Quantity));

            executionReport.SetField(
                new CumQty(0));

            executionReport.SetField(
                new AvgPx(0));

            executionReport.SetField(
                new TransactTime(DateTime.UtcNow));

            if (result.RejectReason is not null)
            {
                executionReport.SetField(
                    new Text(result.RejectReason));
            }

            Session.SendToTarget(executionReport, sessionID);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing order: {ex.Message}");
        }
    }
 
    public void ToApp(Message message, SessionID sessionID)
    {
    }
}