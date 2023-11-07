// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Updater;
using System.Xml.Serialization;

public class UpdatesXml
{
    [XmlArray("Updates")]
    [XmlArrayItem("Update")]
    public List<UpdateInfo> Updates { get; set; } = new();
}
