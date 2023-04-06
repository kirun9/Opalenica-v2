namespace Opalenica.UI.Tiles;

public class PaintContext
{
    public Graphics graphics { get; private set; }
    public Size PaintArea { get; private set; }
    public bool DebugMode { get; private set; }

    public static implicit operator Rectangle(PaintContext context) => new Rectangle(new Point(0, 0), context.PaintArea);
    public static implicit operator Graphics(PaintContext context) => context.graphics;
}