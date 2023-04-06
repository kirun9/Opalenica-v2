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

    public Tile BuildTile()
    {
        TileManager.RegisterTile(Tile);
        return Tile;
    }
}