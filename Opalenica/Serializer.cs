namespace Opalenica;

using Newtonsoft.Json;

using Opalenica.UI.Tiles;

public static class TileViewSerializer
{
    public static string Serialize(TileView tileView)
    {
        return JsonConvert.SerializeObject(tileView, Formatting.Indented, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            NullValueHandling = NullValueHandling.Include,
            DefaultValueHandling = DefaultValueHandling.Include
        });
    }

    public static TileView Deserialize(string serializedTileView)
    {
        return JsonConvert.DeserializeObject<TileView>(serializedTileView, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            NullValueHandling = NullValueHandling.Include,
            DefaultValueHandling = DefaultValueHandling.Include
        });
    }
}
