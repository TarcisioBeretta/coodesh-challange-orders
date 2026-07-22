using QuickFix;

namespace OrderAccumulator.Infrastructure.Fix;

public class FixApplication : IApplication
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

        // Converter FIX -> Command
    }

    public void ToApp(Message message, SessionID sessionID)
    {
    }
}