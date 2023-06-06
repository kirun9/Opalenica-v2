namespace Opalenica.UI.Tiles;

using Opalenica.Elements;
using Opalenica.UI.Tiles.Interfaces;

using System.Windows.Forms;

public class TrackTile : Tile, IContextMenu
{
    public Track Track { get; set; }
    public bool IsHorizontal { get; set; }
    public Location StartLocation { get; set; } = Tiles.Location.None;
    public Location EndLocation { get; set; } = Tiles.Location.None;

    public override void Paint(PaintContext context)
    {
        base.Paint(context);
        using Pen pen = Track.GetPen(context.Pulse);
        if (StartLocation is Tiles.Location.None && EndLocation is Tiles.Location.None)
        {
            context.Graphics.DrawLine(pen, IsHorizontal ? 0 : context.PaintArea.Width / 2, IsHorizontal ? context.PaintArea.Height / 2 : 0, IsHorizontal ? context.PaintArea.Width : context.PaintArea.Width / 2, IsHorizontal ? context.PaintArea.Height / 2 : context.PaintArea.Height);
            return;
        }

        if ((StartLocation.HasFlag(Tiles.Location.Top) && EndLocation.HasFlag(Tiles.Location.Top)) ||
            (StartLocation.HasFlag(Tiles.Location.Bottom) && EndLocation.HasFlag(Tiles.Location.Bottom)) ||
            (StartLocation.HasFlag(Tiles.Location.Left) && EndLocation.HasFlag(Tiles.Location.Left)) ||
            (StartLocation.HasFlag(Tiles.Location.Right) && EndLocation.HasFlag(Tiles.Location.Right)))
            throw new ArgumentException("Start and End locations cannot be on the same side");
        if (StartLocation == EndLocation)
            throw new ArgumentException("Start and End locations cannot be in the same location");

        Point startPoint = new Point(
            StartLocation switch
            {
                var l when l.HasFlag(Tiles.Location.Left) => 0,
                var l when l.HasFlag(Tiles.Location.MiddleH) => context.PaintArea.Width / 2,
                var l when l.HasFlag(Tiles.Location.Right) => context.PaintArea.Width,
                _ => throw new ArgumentException("Invalid state of" + nameof(StartLocation)),
            },
            StartLocation switch
            {
                var l when l.HasFlag(Tiles.Location.Top) => 0,
                var l when l.HasFlag(Tiles.Location.MiddleV) => context.PaintArea.Height / 2,
                var l when l.HasFlag(Tiles.Location.Bottom) => context.PaintArea.Height,
                _ => throw new ArgumentException("Invalid state of" + nameof(StartLocation)),
            });
        Point endPoint = new Point(
            EndLocation switch
            {
                var l when l.HasFlag(Tiles.Location.Left) => 0,
                var l when l.HasFlag(Tiles.Location.MiddleH) => context.PaintArea.Width / 2,
                var l when l.HasFlag(Tiles.Location.Right) => context.PaintArea.Width,
                _ => throw new ArgumentException("Invalid state of" + nameof(EndLocation)),
            },
            EndLocation switch
            {
                var l when l.HasFlag(Tiles.Location.Top) => 0,
                var l when l.HasFlag(Tiles.Location.MiddleV) => context.PaintArea.Height / 2,
                var l when l.HasFlag(Tiles.Location.Bottom) => context.PaintArea.Height,
                _ => throw new ArgumentException("Invalid state of" + nameof(EndLocation)),
            });
        context.Graphics.DrawLine(pen, startPoint, endPoint);
    }

    public ContextMenuStrip GetMenuStrip()
    {
        ContextMenuStrip strip = new ContextMenuStrip();
        ToolStripMenuItem item = new ToolStripMenuItem("Test");
        strip.Items.Add(item);
        return strip;
    }
}