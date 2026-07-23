using QuickFix;
using QuickFix.Fields;

namespace OrderGenerator.Infrastructure.Fix;

public class FixApplication(PendingExecutionReportStore pendingStore) : IApplication
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
        var msgType = new MsgType();

        message.Header.GetField(msgType);

        if (msgType.Value == MsgType.EXECUTION_REPORT)
        {
            var executionReport = (QuickFix.FIX44.ExecutionReport)message;

            var clOrdId = executionReport.GetString(Tags.ClOrdID);

            var orderId = Guid.Parse(clOrdId);

            pendingStore.Complete(orderId, executionReport);
        }
    }

    public void ToApp(Message message, SessionID sessionID)
    {
    }
}