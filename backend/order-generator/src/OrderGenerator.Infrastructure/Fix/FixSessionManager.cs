using QuickFix;
using QuickFix.Logger;
using QuickFix.Store;
using QuickFix.Transport;

namespace OrderGenerator.Infrastructure.Fix;

public class FixSessionManager
{
    private SocketInitiator? _initiator;

    public void Start()
    {
        var configPath = Path.Combine(
            AppContext.BaseDirectory,
            "Fix",
            "initiator.cfg");

        var settings = new SessionSettings(configPath);

        var fix44Path = Path.Combine(
            AppContext.BaseDirectory,
            "Fix",
            "FIX44.xml");

        foreach (var sessionId in settings.GetSessions())
        {
            var sessionSettings = settings.Get(sessionId);

            sessionSettings.SetString(
                "DataDictionary",
                fix44Path);
        }

        var application = new FixApplication();

        var storeFactory = new FileStoreFactory(settings);

        var logFactory = new FileLogFactory(settings);

        _initiator =
            new SocketInitiator(
                application,
                storeFactory,
                settings,
                logFactory);

        _initiator.Start();

        Console.WriteLine("FIX Initiator started");
    }

    public void Stop()
    {
        if (_initiator is null)
        {
            return;
        }

        _initiator.Stop();

        Console.WriteLine("FIX Initiator stopped");
    }
}