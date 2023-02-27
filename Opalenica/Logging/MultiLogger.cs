namespace Opalenica.Logging;

internal class MultiLogger : ILogger
{
    private ConsoleLogger _consoleLogger;
    private FileLogger _fileLogger;

    public MultiLogger()
    {
        _consoleLogger = new ConsoleLogger();
        _fileLogger = new FileLogger();
    }

    public void Log(IMessage message)
    {
        _consoleLogger.Log(message);
        _fileLogger.Log(message);
    }
}
