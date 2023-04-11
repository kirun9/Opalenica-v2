﻿namespace Opalenica.UI.Tiles;

using System.Drawing.Drawing2D;
using System.Windows.Forms;

using Opalenica.Commands;
using Opalenica.UI.Tiles.Interfaces;
using static Opalenica.UI.Colors;

public class TempTile : Tile, ILeftMouseAction, IContextMenu
{
    public bool Selected { get; set; }

    public override void Paint(PaintContext context)
    {
        base.Paint(context);
        var g = context.Graphics;
        var Font = SystemFonts.DefaultFont;

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
        item.Click += (_, _) => CommandManager.ExecuteCommand($"fullscreen", new InternalCommandSender() { IsAdmin = true, ID = $"{this.GetType().Name}.GetMenuStrip.Fullscreen" });
        strip.Items.Add(item);
        return strip;
    }
}