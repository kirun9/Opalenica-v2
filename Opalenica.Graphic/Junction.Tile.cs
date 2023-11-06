namespace Opalenica.Graphic;

using Opalenica.Graphic.Base;
using Opalenica.Graphic.Base.Interfaces;

using System.Windows.Forms;

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
}
