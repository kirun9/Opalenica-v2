namespace Opalenica.UI.Tiles;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Reflection;
using Opalenica.Elements;
using Newtonsoft.Json.Serialization;

public partial class TileView
{

    public class CustomContractResolver : DefaultContractResolver
    {
        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            var members = base.GetSerializableMembers(objectType);

            // Filter out members marked with [JsonIgnore] attribute
            members.RemoveAll(member => member.GetCustomAttributes(typeof(JsonIgnoreAttribute), true).Any());

            return members;
        }
    }

    public class TileViewJsonConverter : JsonConverter<TileView>
    {
        private static readonly string ElementsObjectName = "Elements";

        public override void WriteJson(JsonWriter writer, TileView? value, JsonSerializer serializer)
        {
            var contractResolver = new CustomContractResolver();
            var contract = contractResolver.ResolveContract(typeof(Tile));

            var valueProvider = contract.DefaultCreator != null
                ? contract.DefaultCreator()
                : contract.CreatedType.GetConstructor(Type.EmptyTypes)?.Invoke(null);

            serializer.ContractResolver = contractResolver;

            writer.WriteStartObject();
            writer.WritePropertyName(ElementsObjectName);
            writer.WriteStartArray();
            foreach (var element in value.SearchElements())
            {
                serializer.Serialize(writer, element);
            }
            writer.WriteEndArray();
            writer.WritePropertyName(nameof(TileView.ViewID));
            writer.WriteValue(value.ViewID);
            writer.WritePropertyName(nameof(TileView.Size));
            serializer.Serialize(writer, value.Size);
            writer.WritePropertyName(nameof(TileView.ViewType));
            writer.WriteValue(Enum.GetName(typeof(ViewType), value.ViewType));
            writer.WritePropertyName(nameof(TileView.Tiles));
            writer.WriteStartArray();
            for (int i = 0; i < value.Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < value.Tiles.GetLength(1); j++)
                {
                    var tile = value.Tiles[i, j];
                    if (tile is not null and not EmptyTile and not OccupiedTile && tile.GetType().GetCustomAttribute<DoNotSaveTileAttribute>() is null)
                    {
                        serializer.Serialize(writer, tile);
                    }
                }
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
        }

        public override TileView? ReadJson(JsonReader reader, Type objectType, TileView? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            JArray elements = (JArray)jsonObject[ElementsObjectName];
            foreach (var element in elements)
            {
                if (element.Type != JTokenType.Null)
                {
                    Element.RegisterElement(serializer.Deserialize<Element>(element.CreateReader()));
                }
            }

            var viewId = jsonObject[nameof(TileView.ViewID)].Value<string>();
            var size = jsonObject[nameof(TileView.Size)].ToObject<Size>();
            var viewType = Enum.Parse<ViewType>(jsonObject[nameof(TileView.ViewType)].Value<string>());

            var tileArray = new Tile[size.Width, size.Height];
            JArray tiles = (JArray)jsonObject[nameof(TileView.Tiles)];
            foreach (var tile in tiles)
            {
                if (tile.Type != JTokenType.Null)
                {
                    var deserializedTile = (Tile)serializer.Deserialize(tile.CreateReader(), typeof(Tile));
                    if (deserializedTile is not null and not EmptyTile and not OccupiedTile && deserializedTile.GetType().GetCustomAttribute<DoNotSaveTileAttribute>() is null)
                    {
                        var name = deserializedTile.Name;
                        var locations = deserializedTile.Locations;
                        var tileSize = deserializedTile.TileSize;

                        foreach (var location in locations)
                        {
                            var point = location.Value;
                            if (point.X >= 0 && point.X < size.Width && point.Y >= 0 && point.Y < size.Height)
                            {
                                deserializedTile.Name = name;
                                deserializedTile.Locations = locations;
                                deserializedTile.TileSize = tileSize;
                                tileArray[point.X, point.Y] = deserializedTile;
                            }
                        }
                    }
                }
            }

            return new TileView(viewId, size)
            {
                ViewType = viewType,
                Tiles = tileArray
            };
        }
    }
}