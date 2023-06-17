namespace Opalenica;

using Newtonsoft.Json;

using Opalenica.Elements;
using Opalenica.UI.Tiles;

using System.Collections.Generic;
using System.Reflection;

public static class TileViewSerializer
{
    public static string Serialize(TileView tileView)
    {
        return JsonConvert.SerializeObject(tileView, Formatting.Indented, GetSettings());
    }

    public static TileView Deserialize(string serializedTileView)
    {
        return JsonConvert.DeserializeObject<TileView>(serializedTileView, GetSettings());
    }

    private static JsonSerializerSettings GetSettings()
    {
        var settings = new JsonSerializerSettings();
        settings.Converters = GetConverters();
        settings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
        settings.NullValueHandling = NullValueHandling.Ignore;
        settings.TypeNameHandling = TypeNameHandling.Auto;
        return settings;
    }

    private static IList<JsonConverter> GetConverters()
    {
        List<JsonConverter> output = new List<JsonConverter>();
        foreach (var type in Assembly.GetEntryAssembly().DefinedTypes)
        {
            if (type.IsNestedPublic)
            {
                if (type.IsAssignableTo(typeof(JsonConverter)))
                {
                    var constr = type.DeclaredConstructors.Where(e => !e.IsStatic).First();
                    if (constr is null) continue;
                    output.Add((JsonConverter)constr.Invoke(new object[] {}));
                }
            }
        }
        return output;
    }
}
