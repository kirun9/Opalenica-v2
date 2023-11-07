// Copyright (c) PKMK. All rights reserved.

namespace Opalenica.UI;

using Opalenica.Graphic;
using Opalenica.Graphic.Interfaces;
using Opalenica.UI.Tiles.ElementTiles;
using Opalenica.UI.Tiles.Interfaces;

using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class Pulpit : Control
{
    private bool DesignerMode { get; } = false;
    public static (float Horizontal, float Vertical) PulpitScale { get; private set; } = (1, 1);
    private static readonly Size designSize = new Size(1366, 768);

    Stopwatch watch = new Stopwatch();

    Grid grid = new Grid("34x19,40x40");

    private ContextMenuStrip _contextMenu;
    private bool _blockLeftRightClick;

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
        DoubleBuffered = true;
        watch.Start();

        var view = new TileView("debugView", grid.GridDimensions);

        new Track() { Name = "it101", Data = TrackData.BrakDanych, PermanentData = new Dictionary<string, object>() { { Track.DataNames.Kontrola.ToString(), (object)true }, { Track.DataNames.Zamkniety.ToString(), (object)true } } }.RegisterElement();
        new Track() { Name = "it102", Data = TrackData.BrakDanych, PermanentData = new Dictionary<string, object>() { { Track.DataNames.Kontrola.ToString(), (object)true }, { Track.DataNames.Zamkniety.ToString(), (object)false } } }.RegisterElement();
        new Junction() { Name = "roz1", Data = JunctionData.StanPodstawowy, TrackA = Element.GetElement<Track>("it101"), TrackB = Element.GetElement<Track>("it101"), TrackC = Element.GetElement<Track>("it102"), PermanentData = new Dictionary<string, object>() { { Junction.DataNames.Zamkniety.ToString(), (object)false } } }.RegisterElement();


        view.AddTile(new ColorTile() { Locations = { { "debugView", new Point(1, 0) } }, TileSize = new Size(3, 2), Name = "ColorCheckTile" });
        view.AddTile(new TrackTile() { Locations = { { "debugView", new Point(1, 5) } }, TileSize = new Size(5, 1), Track = Element.GetElement<Track>("it101"), IsHorizontal = true });
        view.AddTile(new TrackTile() { Locations = { { "debugView", new Point(6, 5) } }, TileSize = new Size(5, 1), Track = Element.GetElement<Track>("it102"), IsHorizontal = true });
        //view.AddTile(new TrackTile() { Locations = { { "debugView", new Point(11, 5) } }, TileSize = new Size(1, 1), Track = Element.GetElement<Track>("it102"), StartLocation = Graphic.Base.Location.MiddleLeft, EndLocation = Graphic.Base.Location.BottomMiddle });
        view.AddTile(new JunctionTile() { Locations = { { "debugView", new Point(11, 5) } }, Junction = Element.GetElement<Junction>("roz1"), Rotation = JunctionRotation.JunctionR_Down });
        Element.GetElement<Track>("it102").Data = TrackData.UszkodzenieKontroli;
        //Element.GetElement<Junction>("roz1").Data = JunctionData.Zajety;

        grid.CurrentView = TileViewManager.GetTileView("debugView");
        TileViewManager.SaveViews();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        watch.Restart();
        //Debug.WriteLine(watch.Elapsed.ToString());

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
        string s = $"Dimensions: {Width}x{Height}\nScale: {PulpitScale.Horizontal}x{PulpitScale.Vertical}";
        g.FillRectangle(Brushes.Black, 0, 0, Width, Height);
        using Pen pen = new Pen(Colors.White, 5);
        pen.Alignment = PenAlignment.Inset;
        g.DrawRectangle(pen, 0, 0, Width, Height);

        using var cap = new AdjustableArrowCap(5, 5);
        pen.Alignment = PenAlignment.Center;
        pen.CustomEndCap = cap;
        pen.CustomStartCap = cap;

        g.DrawLine(pen, Width * 0.02f, Height * 0.02f, Width * 0.98f, Height * 0.98f);
        g.DrawLine(pen, Width * 0.02f, Height * 0.98f, Width * 0.98f, Height * 0.02f);

        SizeF size = g.MeasureString(s, Font);
        g.FillRectangle(Brushes.Black, center.X - size.Width / 2, center.Y - size.Height / 2, size.Width, size.Height);
        g.DrawString(s, Font, Brushes.White, center.X - size.Width / 2, center.Y - size.Height / 2);
    }

    protected void DrawPulpit(Graphics g)
    {
        var defaultTransform = g.Transform;
        var prevClipRegion = g.Clip;
        foreach (Tile tile in grid.GetTiles())
        {
            Point p = grid.CalculateGraphicTilePosition(tile);
            g.ScaleTransform(PulpitScale.Horizontal, PulpitScale.Vertical);
            g.TranslateTransform(p.X, p.Y);
            g.TranslateTransform(((Width / PulpitScale.Horizontal) - grid.GridDimensions.Width * grid.TileSize.Width + 1) / 2, ((Height / PulpitScale.Vertical) - grid.GridDimensions.Height * grid.TileSize.Height + 1) / 2);
            var tileSize = new Size(grid.TileSize.Width * tile.TileSize.Width, grid.TileSize.Height * tile.TileSize.Height);
            g.Clip = new Region(new Rectangle(0, 0, tileSize.Width + 1, tileSize.Height + 1));
            g.TranslateClip(p.X == 0 ? -2 : -1, p.Y == 0 ? -2 : -1);
            PaintContext paintContext = new PaintContext(Program.Settings.DebugMode, g, tileSize, grid.Pulse, grid.CurrentView.ViewType);
            tile.Paint(paintContext);
            g.Clip = prevClipRegion;
            g.ResetTransform();
        }
        g.ResetTransform();
        g.Transform = defaultTransform;
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        base.OnSizeChanged(e);
        CalculateScale();
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        base.OnMouseClick(e);
        var pos = e.Location;
        pos.X = (int)(pos.X / PulpitScale.Horizontal);
        pos.Y = (int)(pos.Y / PulpitScale.Vertical);
        var tile = grid.GetTileFromPoint(pos);

        if (tile is ILeftMouseAction leftMouseAction && e.Button is MouseButtons.Left)
        {
            if (_blockLeftRightClick)
            {
                _blockLeftRightClick = false;
                return;
            }
            leftMouseAction.LeftClickAction(e);
            return;
        }
        if (tile is IRightMouseAction rightMouseAction && e.Button is MouseButtons.Right)
        {
            if (_blockLeftRightClick)
            {
                _blockLeftRightClick = false;
                return;
            }
            rightMouseAction.RightClickAction(e);
            return;
        }
        if (tile is IContextMenu contextMenuTile && e.Button is MouseButtons.Right)
        {
            _blockLeftRightClick = true;
            _contextMenu = contextMenuTile.GetMenuStrip();
            _contextMenu.Renderer = new Kirun9.Renderers.VS2019DarkBlueRenderer();
            _contextMenu.Show(this, e.Location);
        }

        /*Graphics g = CreateGraphics();
        var defaultTransform = g.Transform;
        g.ScaleTransform(Scale.Horizontal, Scale.Vertical);
        g.DrawRectangle(Colors.Pens.Red, pos.X, pos.Y, 1, 1);
        g.ResetTransform();
        g.Transform = defaultTransform;*/
    }

    private void CalculateScale()
    {
        PulpitScale = ((float)Width / designSize.Width, (float)Height / designSize.Height);
    }
}