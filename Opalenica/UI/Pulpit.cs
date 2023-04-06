// Copyright (c) PKMK. All rights reserved.

namespace Opalenica.UI;

using System.ComponentModel;
using System.Drawing.Drawing2D;

public class Pulpit : Control
{
    private bool DesignerMode { get; } = false;
    public static (float Horizontal, float Vertical) Scale { get; private set; } = (1, 1);
    private static readonly Size designSize = new Size(1366, 768);

    Grid grid = new Grid("1x1,1x1");

    [Category("Appearance")]
    [Browsable(true)]
    public new Padding Padding { get; set; } = new Padding(3, 3, 3, 3);

    public Pulpit() : this(null, null) { }
    public Pulpit(string text) : this(null, text) { }
    public Pulpit(Control parent) : this(parent, null) { }
    public Pulpit(Control parent, string text) : base(parent, text)
    {
        Parent = parent;
        Text = text;
        DesignerMode = LicenseManager.UsageMode == LicenseUsageMode.Designtime;

        //grid[0,0] = new Tile();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        if (DesignerMode || DesignMode)
        {
            DesignerPaint(e.Graphics);
            return;
        }
        CalculateScale();
        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        e.Graphics.SmoothingMode = SmoothingMode.None;
        e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
        e.Graphics.FillRectangle(Colors.Brushes.Black, ClientRectangle);
        DrawPulpit(e.Graphics);
    }

    protected void DesignerPaint(Graphics g)
    {
        Point center = new Point(Width / 2, Height / 2);
        string s = $"Dimensions: {Width}x{Height}\nScale: {Scale.Horizontal}x{Scale.Vertical}";
        g.FillRectangle(Brushes.Black, 0, 0, Width, Height);
        using Pen pen = new Pen(Colors.White, 5);
        pen.Alignment = PenAlignment.Inset;
        g.DrawRectangle(pen, 0, 0, Width, Height);

        using var cap = new AdjustableArrowCap(5, 5);
        pen.Alignment = PenAlignment.Center;
        pen.CustomEndCap = cap;
        pen.CustomStartCap = cap;

        g.DrawLine(pen, Width * 0.05f, Height * 0.05f, Width * 0.95f, Height * 0.95f);
        g.DrawLine(pen, Width * 0.05f, Height * 0.95f, Width * 0.95f, Height * 0.05f);

        SizeF size = g.MeasureString(s, Font);
        g.FillRectangle(Brushes.Black, center.X - size.Width / 2, center.Y - size.Height / 2, size.Width, size.Height);
        g.DrawString(s, Font, Brushes.White, center.X - size.Width / 2, center.Y - size.Height / 2);
    }

    protected void DrawPulpit(Graphics g)
    {

    }

    protected override void OnSizeChanged(EventArgs e)
    {
        base.OnSizeChanged(e);
        CalculateScale();
    }

    private void CalculateScale()
    {
        Scale = ((float)Width / designSize.Width, (float)Height / designSize.Height);
    }
}