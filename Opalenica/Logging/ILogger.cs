namespace Opalenica.Logging;

public interface ILogger
{
    void Log(IMessage message);
}