namespace Opalenica.UI.Tiles;

public class TileView
{
    public string ViewID { get; set; }
    public Size Size { get; set; }
    public Tile[,] Tiles { get; set; }

    public TileView(string viewID, Size size)
    {
        ViewID = viewID;
        Size = size;
        Tiles = new Tile[size.Width, size.Height];
        TileViewManager.RegisterView(this);
    }

    private Tile GetTile(Point location)
    {
        return Tiles[location.X, location.Y] switch
        {
            OccupiedTile ot => ot.MainTile,
            EmptyTile => new EmptyTile(),
            null => new EmptyTile(),
            var tile => tile
        };
    }

    public void RegisterTile(Tile tile)
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
}