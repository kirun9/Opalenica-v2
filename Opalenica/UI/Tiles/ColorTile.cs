namespace Opalenica.UI.Tiles;

using Opalenica.Commands;
using Opalenica.UI.Tiles.Interfaces;

using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

internal class ColorTile : Tile, IContextMenu
{
    public override void Paint(PaintContext context)
    {
        base.Paint(context);
        var g = context.Graphics;
        var Size = context.PaintArea;
        using SolidBrush brush = new SolidBrush(Colors.Red);
        g.FillRectangle(brush, new Rectangle(0, 0, Size.Width / 3, Size.Height / 2));
        brush.Color = Colors.Green;
        g.FillRectangle(brush, new Rectangle(Size.Width / 3, 0, Size.Width / 3, Size.Height / 2));
        brush.Color = Colors.Blue;
        g.FillRectangle(brush, new Rectangle(Size.Width / 3 * 2, 0, Size.Width / 3, Size.Height / 2));
        brush.Color = Colors.Gray;
        g.FillRectangle(brush, new Rectangle(0, Size.Height / 2, Size.Width / 3, Size.Height / 2));
        brush.Color = Colors.White;
        g.FillEllipse(brush, new Rectangle(0, Size.Height / 2, Size.Width / 3, Size.Height / 2));
        brush.Color = context.Pulse ? Colors.White : Colors.Gray;
        g.FillRectangle(brush, new Rectangle(Size.Width / 3, Size.Height / 2, Size.Width / 3, Size.Height / 2));
    }

    public ContextMenuStrip GetMenuStrip()
    {
        ContextMenuStrip strip = new ContextMenuStrip();
        ToolStripMenuItem item = new ToolStripMenuItem("Przerysuj Ekran");
        item.Click += (_, _) => CommandManager.ExecuteCommand($"RepaintScreen", new InternalCommandSender() { IsAdmin = true, ID = $"{this.GetType().Name}.GetMenuStrip.RepaintScreen" });
        strip.Items.Add(item);
        return strip;
    }
}
