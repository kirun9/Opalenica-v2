namespace Opalenica.Logging;

using System;
using System.Runtime.CompilerServices;
using System.Text;

public class LogMessage : IMessage
{
    public String Message { get; }
    public DateTime TimeStamp { get; }
    public MessageLevel Severity { get; }
    public String Source { get; }

    public LogMessage(string message, string source, MessageLevel severity = MessageLevel.Info, [CallerMemberName] string callerName = "")
    {
        Message = message;
        TimeStamp = DateTime.UtcNow;
        Severity = severity;
        Source = source is null or "" ? callerName : source;
    }

    public string ToLogString(string DateTimeFormat, int severityLength, int sourceLength, int messageLength)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("[{0:" + DateTimeFormat + "}] " , TimeStamp);
        sb.AppendFormat("[{0:" + severityLength + "}] ", Severity.ToString()[..Math.Min(severityLength, Severity.ToString().Length)]);
        sb.AppendFormat("[{0:" + sourceLength + "}] ", Source[..Math.Min(sourceLength, Source.Length)]);

        int currentLineLength = sb.ToString().Length;
        int allowedLineLength = DateTimeFormat.Length + severityLength + sourceLength + messageLength;

        int currentWordLength = 0;

        foreach (string word in Message.Split(" "))
        {
            if (currentLineLength + currentWordLength + 1 > allowedLineLength)
            {
                sb.Append("\n" + new string(' ', DateTimeFormat.Length + severityLength + sourceLength + 2));
                currentLineLength = DateTimeFormat.Length + severityLength + sourceLength;
            }

            sb.Append(" " + word);
            currentWordLength = word.Length;
            currentLineLength += currentWordLength;
        }

        return sb.ToString().Trim();
    }
}