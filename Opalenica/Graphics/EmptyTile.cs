// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Graphic;

using System.Drawing.Drawing2D;

using static Opalenica.Graphic.Colors;

public sealed class EmptyTile : Tile
{
    public override void Paint(PaintContext context)
    {
        base.Paint(context);
        if (!context.DebugMode)
            return;
        var g = context.Graphics;
        var Font = SystemFonts.DefaultFont;
        using Font font = new Font(Font.FontFamily, 7, Font.Style, Font.Unit, Font.GdiCharSet, Font.GdiVerticalFont);
        string str = $"{context.PaintArea.Width}x{context.PaintArea.Height}";
        using Pen pen = new Pen(Red, 1);
        pen.Alignment = PenAlignment.Inset;
        g.DrawRectangle(pen, context);
        var s = g.MeasureString(str, font);
        g.DrawString(str, font, Brushes.Red, new PointF((context.PaintArea.Width - s.Width) / 2, (context.PaintArea.Height - s.Height) / 2));
    }
}