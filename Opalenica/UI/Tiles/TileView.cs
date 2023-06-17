namespace Opalenica.UI.Tiles;
using System.Linq;
using Opalenica.Elements;
using Newtonsoft.Json.Serialization;

public partial class TileView
{
    public string ViewID { get; set; }
    public Size Size { get; set; }
    public Tile[,] Tiles { get; set; }
    public ViewType ViewType { get; set; }

    public TileView(string viewID, Size size)
    {
        ViewID = viewID;
        Size = size;
        Tiles = new Tile[size.Width, size.Height];
        TileViewManager.RegisterView(this);
    }

    public IEnumerable<Tile> GetAllTiles()
    {
        for (int i = 0; i < Tiles.GetLength(0); i++)
            for (int j = 0; j < Tiles.GetLength(1); j++)
                yield return GetTile(new Point(i, j));
    }


    public IEnumerable<Tile> GetTiles()
    {
        for (int i = 0; i < Tiles.GetLength(0); i++)
            for (int j = 0; j < Tiles.GetLength(1); j++)
                if (Tiles[i, j] is not null and not EmptyTile)
                    yield return GetTile(new Point(i, j));
    }

    public Tile GetTile(Point location)
    {
        return Tiles[location.X, location.Y] switch
        {
            OccupiedTile ot => ot.MainTile,
            EmptyTile => new EmptyTile() { Locations = { { ViewID, location } }, ViewID = ViewID },
            null => new EmptyTile() { Locations = { { ViewID, location } }, ViewID = ViewID },
            var tile => tile
        };
    }

    public void RemoveTile(Tile tile)
    {
        var location = tile.Locations[ViewID];
        if (GetTile(location) is not null and not EmptyTile)
        {
            for (int x = 0; x < tile.TileSize.Width; x++)
            {
                for (int y = 0; y < tile.TileSize.Height; y++)
                {
                    if (x == 0 && y == 0) continue;
                    Tiles[location.X + x, location.Y + y] = null;
                }
            }
        }
    }

    public void AddTile<T>(T tile) where T : Tile
    {
        var location = tile.Locations[ViewID];
        if (GetTile(location) is not null and not EmptyTile)
            throw new TileAlreadyOccupiedException(location, tile);
        else
        {
            Tiles[location.X, location.Y] = tile;
            for (int x = 0; x < tile.TileSize.Width; x++)
            {
                for (int y = 0; y < tile.TileSize.Height; y++)
                {
                    if (x == 0 && y == 0) continue;
                    Tiles[location.X + x, location.Y + y] = new OccupiedTile() { MainTile = tile };
                }
            }
        }
    }

    private IEnumerable<Element> SearchElements()
    {
        List<Element> elements = new List<Element>();
        foreach (var tile in Tiles)
        {
            if (tile is IHasElements elementTile)
            {
                var tileElements = elementTile.GetElements();
                foreach (var tileElement in tileElements)
                {
                    if (!elements.Contains(tileElement)) elements.Add(tileElement);
                }
            }
        }
        return elements;
    }

}

/*public class TileViewConverter : JsonConverter<TileView>
{
    public override TileView? ReadJson(JsonReader reader, Type objectType, TileView? existingValue, Boolean hasExistingValue, JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);
        TileView tileView = new TileView(jsonObject[nameof(TileView.ViewID)].Value<string>(), jsonObject[nameof(TileView.Size)].ToObject<Size>());
        tileView.ViewType = serializer.Deserialize<ViewType>(jsonObject[nameof(TileView.ViewType)].CreateReader());
        JArray array = (JArray)jsonObject[nameof(TileView.Tiles)];
        foreach (var tile in array)
        {
            if (tile.Type != JTokenType.Null)
                tileView.AddTile(serializer.Deserialize<Tile>(tile.CreateReader()));
        }
        return tileView;
    }

    public override void WriteJson(JsonWriter json, TileView? tileView, JsonSerializer serializer)
    {
        json.WriteStartObject();
        json.WritePropertyName(nameof(TileView.ViewID));
        json.WriteValue(tileView.ViewID);
        json.WritePropertyName(nameof(TileView.Size));
        serializer.Serialize(json, tileView.Size);
        json.WritePropertyName(nameof(TileView.ViewType));
        serializer.Serialize(json, tileView.ViewType);
        json.WritePropertyName(nameof(TileView.Tiles));
        json.WriteStartArray();
        for (int i = 0; i < tileView.Tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tileView.Tiles.GetLength(1); j++)
            {
                var tile = tileView.Tiles[i, j];
                if (tile is not null and not EmptyTile and not OccupiedTile)
                {
                    if (tile.GetType().GetCustomAttribute<DoNotSaveTileAttribute>() is null)
                        serializer.Serialize(json, tile);
                }
            }
        }

        json.WriteEndArray();
        json.WriteEndObject();
    }
}*/