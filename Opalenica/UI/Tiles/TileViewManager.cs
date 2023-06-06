namespace Opalenica.UI.Tiles;

using Kirun9.CommandParser;

using Newtonsoft.Json;

using System.IO.Compression;
using System.Xml.Serialization;

public class TileViewManager
{
    private static Dictionary<string, TileView> registeredViews = new Dictionary<string, TileView>();
    public static void RegisterView(TileView view)
    {
        if (registeredViews.ContainsKey(view.ViewID))
            throw new DuplicateTileViewException(view.ViewID);
        else
        {
            registeredViews.Add(view.ViewID, view);
            foreach (var tile in view.GetTiles())
            {
                TileManager.RegisterTile(tile);
            }
        }
    }

    public static TileView GetTileView(string viewID)
    {
        if (registeredViews.ContainsKey(viewID))
            return registeredViews[viewID];
        else
            throw new TileViewNotFoundException(viewID);
    }

    public static void ReadViews()
    {
        if (!Directory.Exists("views")) return;
#if !DEBUG
        foreach (var file in Directory.GetFiles("views", "*.dat"))
        {
            using StreamReader reader = new StreamReader(new GZipStream(File.OpenRead(file), CompressionMode.Decompress));
            var view = TileViewSerializer.Deserialize(reader.ReadToEnd());
#else
            foreach (var file in Directory.GetFiles("views", "*.json"))
        {
            var view = TileViewSerializer.Deserialize(File.ReadAllText(file));
#endif
            foreach (var tile in view.GetTiles())
            {
                TileManager.RegisterTile(tile);
            }
        }
    }

    public static void SaveViews()
    {
        foreach ((_, var v) in registeredViews)
        {
            if (!Directory.Exists("views"))
                Directory.CreateDirectory("views");
#if !DEBUG
            using StreamWriter writer = new StreamWriter(new GZipStream(File.OpenWrite($"views/{v.ViewID}.dat"), CompressionLevel.Fastest));
            writer.Write(TileViewSerializer.Serialize(v));
            writer.Flush();
#else
            File.WriteAllText($"views/{v.ViewID}.json", TileViewSerializer.Serialize(v));
#endif
        }
    }

    public static void UnregisterViews()
    {
        foreach ((_, var v) in registeredViews)
        {
            TileViewManager.UnregisterView(v);
        }
    }

    public static void UnregisterView(TileView view)
    {
        foreach (var tile in view.GetTiles())
        {
            TileManager.UnregisterTile(tile, view);
        }
        if (registeredViews.ContainsKey(view.ViewID))
            registeredViews.Remove(view.ViewID);
    }
}
