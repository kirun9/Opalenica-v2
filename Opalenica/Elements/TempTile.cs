// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Elements;

using Opalenica.Commands;
using Opalenica.Graphic;
using Opalenica.Graphic.Interfaces;
using Opalenica.UI.Tiles.Interfaces;

using System.Drawing.Drawing2D;
using System.Windows.Forms;

using static Opalenica.Graphic.Colors;

public class TempTile : Tile, ILeftMouseAction, IContextMenu
{
    public bool Selected { get; set; }

    public override void Paint(PaintContext context)
    {
        base.Paint(context);
        var g = context.Graphics;

        using Pen pen = new Pen(Selected ? White : Red, 2);
        pen.Alignment = PenAlignment.Center;
        g.DrawLine(pen, 0, 0, context.PaintArea.Width, context.PaintArea.Height);
    }

    public void LeftClickAction(MouseEventArgs e)
    {
        Selected = true;
    }

    public void RightClickAction(MouseEventArgs e)
    {
        Selected = false;
    }

    public ContextMenuStrip GetMenuStrip()
    {
        ContextMenuStrip strip = new ContextMenuStrip();
        ToolStripMenuItem item = new ToolStripMenuItem("Fullscreen");
        item.Click += (_, _) => CommandManager.ExecuteCommand($"fullscreen", new InternalCommandSender() { IsAdmin = true, ID = $"{GetType().Name}.GetMenuStrip.Fullscreen" });
        strip.Items.Add(item);
        ToolStripMenuItem item2 = new ToolStripMenuItem("SWDR");
        item2.Click += (_, _) => CommandManager.ExecuteCommand($"SWDR", new InternalCommandSender() { IsAdmin = true, ID = $"{GetType().Name}.GetMenuStrip.SWDR" });
        strip.Items.Add(item2);
        return strip;
    }
}