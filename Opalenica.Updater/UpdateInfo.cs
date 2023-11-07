// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Updater;

using System.Xml;
using System.Xml.Serialization;

public class UpdateInfo
{
    [XmlAttribute("AppID")]
    public string ApplicationID { get; set; } = string.Empty;

    [XmlAttribute("ForcedUpdate")]
    public bool ForcedUpdate { get; set; }

    [XmlElement("Version")]
    public string Version { get; set; } = string.Empty;

    [XmlElement]
    public string Location { get; set; } = string.Empty;

    [XmlArray("FileList")]
    [XmlArrayItem("File")]
    public List<FileInfo> FileList { get; set; } = new();

    [XmlElement]
    public FileInfo StartFileInfo { get; set; } = new();
}