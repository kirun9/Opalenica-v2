// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Elements;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;

public partial class Element
{
    // Serializer related class
    public class ElementJsonConverter : JsonConverter<Element>
    {
        public override Element? ReadJson(JsonReader reader, Type objectType, Element? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var element = new Element();
            element.Name = (string)jsonObject["Name"];

            // Do not deserialize PermanentData for now, it will be handled during Tile deserialization

            return element;
        }

        public override void WriteJson(JsonWriter writer, Element? value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Name");
            writer.WriteValue(value.Name);

            // Do not serialize PermanentData for now, it will be handled during Tile serialization

            writer.WriteEndObject();
        }

        public override bool CanRead => true;
        public override bool CanWrite => true;
    }
}
