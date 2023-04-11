namespace Opalenica.UI.Tiles;

public class TileBuilder
{
    private Tile Tile;
    public TileBuilder()
    {
        Tile = new();
    }

    public TileBuilder WithName(string name)
    {
        Tile.Name = name;
        return this;
    }

    public TileBuilder WithLocation(TileView view, Point location)
    {
        Tile.Locations.Add(view.ViewID, location);
        return this;
    }

    public TileBuilder WithTileSize(Size size)
    {
        Tile.TileSize = size;
        return this;
    }

    public Tile BuildTile()
    {
        TileManager.RegisterTile(Tile);
        foreach (var view in Tile.Locations.Keys)
        {
            TileViewManager.GetTileView(view).AddTile(Tile);
        }
        return Tile;
    }
}