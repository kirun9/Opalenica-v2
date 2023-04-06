namespace Opalenica.UI.Tiles;

public class Tile
{
    public string Name { get; set; }
    public Dictionary<string, Point> Locations { get; set; } = new Dictionary<string, Point>();
    public Size TileSize { get; set; }

    public virtual void Paint(PaintContext context)
    {

    }
}