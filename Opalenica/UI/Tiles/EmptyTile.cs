namespace Opalenica.UI.Tiles;

using System.Drawing.Drawing2D;

using static Opalenica.UI.Colors;

public sealed class EmptyTile : Tile
{
    public override void Paint(PaintContext context)
    {
        base.Paint(context);
        var g = context.graphics;
        using Pen pen = new Pen(Red, 2);
        pen.Alignment = PenAlignment.Inset;
        g.DrawRectangle(pen, context);
    }
}