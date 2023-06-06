namespace Opalenica;

using Newtonsoft.Json;

using Opalenica.UI.Tiles;

using static Opalenica.Elements.Element;

public static class TileViewSerializer
{
    public static string Serialize(TileView tileView)
    {
        return JsonConvert.SerializeObject(tileView, Formatting.Indented, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
            Converters = { new TileViewConverter(), new ElementConverter() }
        });
    }

    public static TileView Deserialize(string serializedTileView)
    {
        return JsonConvert.DeserializeObject<TileView>(serializedTileView, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            NullValueHandling = NullValueHandling.Include,
            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
            Converters = { new TileViewConverter(), new ElementConverter() }
        });
    }
}
