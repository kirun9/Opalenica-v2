namespace Opalenica.Graphic;

using Opalenica.Graphic.Base;
using Opalenica.Graphic.Base.Interfaces;

public class TrackTile : Tile, IContextMenu, IHasElements
{
    public Track Track { get; set; }
    public bool IsHorizontal { get; set; }
    public Location StartLocation { get; set; } = Base.Location.None;
    public Location EndLocation { get; set; } = Base.Location.None;

    public override void Paint(PaintContext context)
    {
        base.Paint(context);
        using Pen pen = Track.GetPen(context.Pulse);
        if (StartLocation is Base.Location.None && EndLocation is Base.Location.None)
        {
            context.Graphics.DrawLine(pen, IsHorizontal ? 0 : context.PaintArea.Width / 2, IsHorizontal ? context.PaintArea.Height / 2 : 0, IsHorizontal ? context.PaintArea.Width : context.PaintArea.Width / 2, IsHorizontal ? context.PaintArea.Height / 2 : context.PaintArea.Height);
            return;
        }

        if ((StartLocation.HasFlag(Base.Location.Top) && EndLocation.HasFlag(Base.Location.Top)) ||
            (StartLocation.HasFlag(Base.Location.Bottom) && EndLocation.HasFlag(Base.Location.Bottom)) ||
            (StartLocation.HasFlag(Base.Location.Left) && EndLocation.HasFlag(Base.Location.Left)) ||
            (StartLocation.HasFlag(Base.Location.Right) && EndLocation.HasFlag(Base.Location.Right)))
            throw new ArgumentException("Start and End locations cannot be on the same side");
        if (StartLocation == EndLocation)
            throw new ArgumentException("Start and End locations cannot be in the same location");

        Point startPoint = new Point(
            StartLocation switch
            {
                var l when l.HasFlag(Base.Location.Left) => 0,
                var l when l.HasFlag(Base.Location.MiddleH) => context.PaintArea.Width / 2,
                var l when l.HasFlag(Base.Location.Right) => context.PaintArea.Width,
                _ => throw new ArgumentException("Invalid state of" + nameof(StartLocation)),
            },
            StartLocation switch
            {
                var l when l.HasFlag(Base.Location.Top) => 0,
                var l when l.HasFlag(Base.Location.MiddleV) => context.PaintArea.Height / 2,
                var l when l.HasFlag(Base.Location.Bottom) => context.PaintArea.Height,
                _ => throw new ArgumentException("Invalid state of" + nameof(StartLocation)),
            });
        Point endPoint = new Point(
            EndLocation switch
            {
                var l when l.HasFlag(Base.Location.Left) => 0,
                var l when l.HasFlag(Base.Location.MiddleH) => context.PaintArea.Width / 2,
                var l when l.HasFlag(Base.Location.Right) => context.PaintArea.Width,
                _ => throw new ArgumentException("Invalid state of" + nameof(EndLocation)),
            },
            EndLocation switch
            {
                var l when l.HasFlag(Base.Location.Top) => 0,
                var l when l.HasFlag(Base.Location.MiddleV) => context.PaintArea.Height / 2,
                var l when l.HasFlag(Base.Location.Bottom) => context.PaintArea.Height,
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

    public Element[] GetElements()
    {
        return new[] { Track };
    }
}