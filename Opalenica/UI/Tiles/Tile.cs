namespace Opalenica.UI.Tiles;

using Newtonsoft.Json;

public class Tile
{
    public string Name { get; set; }
    public Dictionary<string, Point> Locations { get; set; } = new Dictionary<string, Point>();
    [JsonIgnore]
    public IEnumerable<string> ViewIDs => Locations.Keys;
    [JsonIgnore]
    public string ViewID { get; set; }
    [JsonIgnore]
    public Point Location => ViewID is not "" or null ? Locations[ViewID] : new Point(0, 0);
    public Size TileSize { get; set; } = new Size(1, 1);

    public virtual void Paint(PaintContext context)
    {

    }

    internal void SetContext(TileView view)
    {
        ViewID = view.ViewID;
    }

    internal bool CheckContext(TileView view)
    {
        return view.ViewID == ViewID;
    }
}

public class ExtendedTile : Tile
{
    public string MoreData { get; set; }
}