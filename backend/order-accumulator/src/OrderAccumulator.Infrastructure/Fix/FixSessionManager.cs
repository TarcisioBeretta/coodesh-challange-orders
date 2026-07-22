using QuickFix;
using QuickFix.Logger;
using QuickFix.Store;

namespace OrderAccumulator.Infrastructure.Fix;

public class FixSessionManager
{
    private ThreadedSocketAcceptor? _acceptor;

    public void Start()
    {
        var configPath = Path.Combine(
            AppContext.BaseDirectory,
            "Fix",
            "acceptor.cfg");

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

        _acceptor = new ThreadedSocketAcceptor(
            application,
            storeFactory,
            settings,
            logFactory);

        _acceptor.Start();

        Console.WriteLine($"FIX Acceptor started using {fix44Path}");
    }

    public void Stop()
    {
        if (_acceptor is null)
        {
            return;
        }

        _acceptor.Stop();

        Console.WriteLine("FIX Acceptor stopped");
    }
}