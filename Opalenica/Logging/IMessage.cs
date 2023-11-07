namespace Opalenica.Logging;

using System;

public interface IMessage
{
    string Message { get; }
    DateTime TimeStamp { get; }
    MessageLevel Severity { get; }
    string Source { get; }
    string ToLogString(string DateTimeFormat, int severityLength, int sourceLength, int messageLength);
}
