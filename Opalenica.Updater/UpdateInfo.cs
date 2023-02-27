namespace Opalenica.Updater;

using System.Xml;
using System.Xml.Serialization;

public class UpdateInfo
{
    [XmlAttribute("AppID")]
    public string ApplicationID { get; set; }

    [XmlElement("Version")]
    public string Version { get; set; }

    [XmlElement]
    public string Location { get; set; }

    [XmlArray("FileList")]
    [XmlArrayItem("File")]
    public List<FileInfo> FileList { get; set; }

    [XmlElement]
    public FileInfo StartFileInfo { get; set; }
}