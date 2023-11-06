namespace Opalenica.Graphic.Base;
using Newtonsoft.Json;

[Flags]
[JsonConverter(typeof(LocationConverter))]
public enum Location
{
    None = 0b000000,

    Top = 0b001_000,
    MiddleV = 0b010_000,
    Bottom = 0b100_000,
    Left = 0b000_001,
    MiddleH = 0b000_010,
    Right = 0b000_100,

    TopLeft = 0b001_001,
    TopMiddle = 0b001_010,
    TopRight = 0b001_100,
    MiddleLeft = 0b010_001,
    MiddleMiddle = 0b010_010,
    MiddleRight = 0b010_100,
    BottomLeft = 0b100_001,
    BottomMiddle = 0b100_010,
    BottomRight = 0b100_100,
}