// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Elements;

using Opalenica.Graphic;
using Opalenica.Graphic.Interfaces;
using System.Windows.Forms;

using static Opalenica.Elements.Junction;

public class JunctionTile : Tile, IContextMenu, IHasElements
{
    public Junction Junction { get; set; }

    public JunctionRotation Rotation { get; set; }

    public override void Paint(PaintContext context)
    {
        base.Paint(context);

        /***
         * Tutaj możesz pisać dalej
         */

    }

    public Element[] GetElements()
    {
        return new[] { Junction };
    }

    public ContextMenuStrip GetMenuStrip()
    {
        ContextMenuStrip strip = new ContextMenuStrip();
        ToolStripMenuItem item = new ToolStripMenuItem("Menu Test");
        strip.Items.Add(item);
        return strip;
    }

    public bool IsSelected()
    {
        return Junction.IsSelected;
    }
}
