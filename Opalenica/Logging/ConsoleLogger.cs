namespace Opalenica.Logging;

using System;

internal class ConsoleLogger : ILogger
{
    public void Log(IMessage message)
    {
        Console.WriteLine(message.ToLogString("HH:mm:ss:fff", 10, 10, 35));
    }
}