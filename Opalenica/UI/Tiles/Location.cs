namespace Opalenica.UI.Tiles;

using Newtonsoft.Json;
using System;

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

public class LocationConverter : JsonConverter<Location>
{
    public override Location ReadJson(JsonReader reader, Type objectType, Location existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Integer)
        {
            var value = Enum.Parse<Location>(reader.ReadAsString());
            if (IsValidLocation((int)value))
            {
                return value;
            }
        }
        throw new JsonException("Invalid location value.");
    }

    public override void WriteJson(JsonWriter writer, Location value, JsonSerializer serializer)
    {
        if (IsValidLocation((int)value))
        {
            writer.WriteValue(Enum.GetName(value));
        }
        else
        {
            throw new JsonException("Invalid location value.");
        }
    }

    private bool IsValidLocation(int value)
    {
        bool hasTopMiddleBottom = (value & 0b111_000) != 0;
        bool hasLeftMiddleRight = (value & 0b000_111) != 0;

        return hasTopMiddleBottom && hasLeftMiddleRight && Enum.IsDefined((Location)value);
    }
}
