// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Elements;

using Opalenica.Graphic;
using Opalenica.Graphic.Interfaces;

public class TrackTile : Tile, IContextMenu, IHasElements
{
    public Track Track { get; set; }
    public bool IsHorizontal { get; set; }
    public Track.Location StartLocation { get; set; } = Track.Location.None;
    public Track.Location EndLocation { get; set; } = Track.Location.None;

    public override void Paint(PaintContext context)
    {
        base.Paint(context);
        using Pen pen = Track.GetPen(context.Pulse);
        if (StartLocation is Track.Location.None && EndLocation is Track.Location.None)
        {
            context.Graphics.DrawLine(pen, IsHorizontal ? 0 : context.PaintArea.Width / 2, IsHorizontal ? context.PaintArea.Height / 2 : 0, IsHorizontal ? context.PaintArea.Width : context.PaintArea.Width / 2, IsHorizontal ? context.PaintArea.Height / 2 : context.PaintArea.Height);
            return;
        }

        if (StartLocation.HasFlag(Track.Location.Top) && EndLocation.HasFlag(Track.Location.Top) ||
            StartLocation.HasFlag(Track.Location.Bottom) && EndLocation.HasFlag(Track.Location.Bottom) ||
            StartLocation.HasFlag(Track.Location.Left) && EndLocation.HasFlag(Track.Location.Left) ||
            StartLocation.HasFlag(Track.Location.Right) && EndLocation.HasFlag(Track.Location.Right))
            throw new ArgumentException("Start and End locations cannot be on the same side");
        if (StartLocation == EndLocation)
            throw new ArgumentException("Start and End locations cannot be in the same location");

        Point startPoint = new Point(
            StartLocation switch
            {
                var l when l.HasFlag(Track.Location.Left) => 0,
                var l when l.HasFlag(Track.Location.MiddleH) => context.PaintArea.Width / 2,
                var l when l.HasFlag(Track.Location.Right) => context.PaintArea.Width,
                _ => throw new ArgumentException("Invalid state of" + nameof(StartLocation)),
            },
            StartLocation switch
            {
                var l when l.HasFlag(Track.Location.Top) => 0,
                var l when l.HasFlag(Track.Location.MiddleV) => context.PaintArea.Height / 2,
                var l when l.HasFlag(Track.Location.Bottom) => context.PaintArea.Height,
                _ => throw new ArgumentException("Invalid state of" + nameof(StartLocation)),
            });
        Point endPoint = new Point(
            EndLocation switch
            {
                var l when l.HasFlag(Track.Location.Left) => 0,
                var l when l.HasFlag(Track.Location.MiddleH) => context.PaintArea.Width / 2,
                var l when l.HasFlag(Track.Location.Right) => context.PaintArea.Width,
                _ => throw new ArgumentException("Invalid state of" + nameof(EndLocation)),
            },
            EndLocation switch
            {
                var l when l.HasFlag(Track.Location.Top) => 0,
                var l when l.HasFlag(Track.Location.MiddleV) => context.PaintArea.Height / 2,
                var l when l.HasFlag(Track.Location.Bottom) => context.PaintArea.Height,
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

    public Boolean IsSelected()
    {
        throw new NotImplementedException();
    }
}