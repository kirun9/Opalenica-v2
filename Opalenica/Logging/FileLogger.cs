namespace Opalenica.Logging;

using System;
using System.Text;
using System.Xml;

internal class FileLogger : ILogger
{
    private string _logFile;

    internal FileLogger()
    {
        _logFile = Path.Combine("logs", DateTime.Now.ToString("yyyy/MM/dd") + ".log");
    }

    public void Log(IMessage message)
    {

        if (!File.Exists(_logFile))
        {
            // Create new xml file
            FileInfo f = new FileInfo(_logFile);
            if (!f.Directory.Exists) f.Directory.Create();
            f.Create().Close();

            XmlTextWriter writer = new XmlTextWriter(_logFile, Encoding.UTF8);
            writer.WriteStartElement("Messages");
            writer.WriteEndElement();
            writer.Close();
        }

        //Append message to xml file
        XmlDocument document = new XmlDocument();
        document.Load(_logFile);
        XmlElement messageElement = document.CreateElement("Message");

        XmlElement messageTextElement = document.CreateElement("MessageText");
        messageTextElement.InnerText = message.Message;
        messageElement.AppendChild(messageTextElement);

        XmlElement timestampElement = document.CreateElement("TimeStamp");
        timestampElement.InnerText = message.TimeStamp.ToString("yyyy/MM/dd HH:mm:ss:fff");
        messageElement.AppendChild(timestampElement);

        XmlElement severityElement = document.CreateElement("Severity");
        severityElement.InnerText = message.Severity.ToString();
        messageElement.AppendChild(severityElement);

        XmlElement sourceElement = document.CreateElement("Source");
        sourceElement.InnerText = message.Source;
        messageElement.AppendChild(sourceElement);

        document.DocumentElement.AppendChild(messageElement);
        document.Save(_logFile);
    }
}
