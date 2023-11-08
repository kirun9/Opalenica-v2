// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Elements;

using Newtonsoft.Json;
public partial class Track
{
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
}
