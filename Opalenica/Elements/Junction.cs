// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Elements;

using Newtonsoft.Json;
using Opalenica.Graphic;
using System.Drawing.Drawing2D;

public partial class Junction : Element
{
    public enum DataNames
    {
        Zamkniety
    }

    public JunctionData Data { get; set; }

    public Track TrackA { get; set; }
    public Track TrackB { get; set; }
    public Track TrackC { get; set; }


    [JsonIgnore]
    public bool Zamkniety
    {
        get => (bool)this[DataNames.Zamkniety];
        set => this[DataNames.Zamkniety] = value;
    }

    public Junction()
    {
        this[DataNames.Zamkniety] = default(bool);
    }

    public object this[DataNames key]
    {
        get
        {
            if (!PermanentData.ContainsKey(Enum.GetName(key))) this[key] = default;
            return PermanentData.GetValueOrDefault(Enum.GetName(key), default);
        }
        set => PermanentData[Enum.GetName(key)] = value;
    }

    public Pen GetPen(bool pulse)
    {
        var pw = (bool)this[DataNames.Zamkniety] ? 4 : 2;
        return new Pen(GetColor(pulse))
        {
            DashStyle = DashStyle.Solid,
            Width = pw
        };
    }

    private Color GetColor(bool pulse)
    {
        return Data switch
        {
            JunctionData.BrakDanych => Colors.White,
            JunctionData.Rozprucie when !pulse => Colors.Red,
            JunctionData.Rozprucie when pulse => Colors.Gray,
            JunctionData.NieoczekiwanyBrakKontroli when !pulse => Colors.White,
            JunctionData.NieoczekiwanyBrakKontroli when pulse => Colors.Gray,
            JunctionData.BrakKontroli => Colors.Invisible, // (Niewidoczny) Albo Colors.Black -- nie testowane w pełni
            JunctionData.Zajety => Colors.Red,
            JunctionData.ZwalnianyCzasowo => Colors.Pink,
            JunctionData.PrzebiegPociagowy => Colors.Green,
            JunctionData.PrzebiegManewrowy => Colors.Yellow,
            JunctionData.OchronaPrzebiegu => Colors.Yellow,
            JunctionData.OchronaBoczna => Colors.Yellow,
            JunctionData.StopPolozenie => Colors.Pink,
            JunctionData.RejonManewrowy => Colors.LightCyan,
            JunctionData.StanPodstawowy => Colors.Gray,
            _ => Colors.None
        };
    }
}
