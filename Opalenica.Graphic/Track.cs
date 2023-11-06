namespace Opalenica.Graphic;

using Opalenica.Graphic.Base;

using System.Drawing.Drawing2D;
using System.Drawing;

using Newtonsoft.Json;

public class Track : Element
{
    public enum DataNames
    {
        Zamkniety,
        Kontrola
    }

    public TrackData Data { get; set; }
    public TrackType Type { get; set; }

    [JsonIgnore]
    public bool Zamkniety
    {
        get => (bool)this[DataNames.Zamkniety];
        set => this[DataNames.Zamkniety] = value;
    }

    [JsonIgnore]
    public bool Kontrola
    {
        get => (bool)this[DataNames.Kontrola];
        set => this[DataNames.Kontrola] = value;
    }

    public Track()
    {
        this[DataNames.Kontrola] = default(bool);
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
        var pw = ((bool)this[DataNames.Zamkniety]) ? 4 : 2;
        return new Pen(GetColor(pulse))
        {
            DashStyle = DashStyle.Custom,
            Width = pw,
            CompoundArray = Zamkniety ? new float[] { 0f, 1f / pw, 3f / pw, 1f } : new float[] { 0, 1 },
            DashPattern = (!Kontrola) ? new float[] { 1f, 2f, 1f } : new float[] { 1f }
        };
    }

    private Color GetColor(bool pulse)
    {
        return Data switch
        {
            TrackData.StanPodstawowy                     => Colors.Gray,
            TrackData.RejonManewrowy                     => Colors.LightCyan,
            TrackData.OchronaPrzebiegu                   => Colors.Yellow,
            TrackData.PrzebiegManewrowy                  => Colors.Yellow,
            TrackData.ZwalnianyCzasowo                   => Colors.Pink,
            TrackData.Zajety                             => Colors.Red,
            TrackData.PierwszyPrzejazd                   => Colors.DarkRed,
            TrackData.PotwierdzenieZerowania when !pulse => Colors.Red,
            TrackData.PotwierdzenieZerowania when pulse  => Colors.Gray,
            TrackData.UszkodzenieKontroli when !pulse    => Colors.White,
            TrackData.UszkodzenieKontroli when pulse     => Colors.Red,
            TrackData.BrakDanych                         => Colors.White,
            _ => Colors.None
        };
    }
}