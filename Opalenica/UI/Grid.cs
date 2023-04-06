namespace Opalenica.UI;

using Opalenica.UI.Tiles;

using System.Text.RegularExpressions;
using Timer = System.Windows.Forms.Timer;

public class Grid
{
    public const string SizeRegex = @"^([1-9][0-9]*)x([1-9][0-9]*)[,]([1-9][0-9]*)x([1-9][0-9]*)$";
    private Timer _timer = new Timer();
    public bool Pulse { get; private set; }
    public Size GridDimensions { get; private set; }
    public Size TileSize { get; set; }
    public bool DebugMode => Program.Settings.DebugMode;
    public Padding Padding { get; set; }

    public TileView CurrentView { get; private set; }

    public void LoadView(TileView view)
    {

    }

    public Grid(string size)
    {
        if (!Regex.IsMatch(size, SizeRegex)) throw new ArgumentException("", nameof(size));
        _timer = new Timer();
        _timer.Interval = 500; // 1Hz, wypełnienie 50/50%
        _timer.Tick += (_, _) => { Pulse = !Pulse; };
        _timer.Enabled = true;

        var sizes = Regex.Split(size, SizeRegex, RegexOptions.IgnoreCase);
        if (sizes.Length is not 4) throw new ArgumentException("", nameof(size));
        GridDimensions = new Size(Int32.Parse(sizes[0]), Int32.Parse(sizes[1]));
        TileSize = new Size(Int32.Parse(sizes[2]), Int32.Parse(sizes[3]));
    }
}

/*
public class PanelView
{
    public (string Width, string Height) GridSize { get; set; }
    public ViewType ViewType { get; set; }
    private Tile[,] _tiles;

    public Tile this[int pos]
    {
        get { return _tiles.GetValue(pos) as Tile; }
        set { _tiles.SetValue(value, pos); }
    }

    public Tile this[int x, int y]
    {
        get { return _tiles[x, y]; }
        set { _tiles[x, y] = value; }
    }
}

public enum ViewType
{
    Normal,
    Advanced
}

public class Tile
{
    public virtual void OnPaint(DrawingContext drawingContext)
    {
        Graphics g = drawingContext.Graphics;
        using Pen p = new Pen(Colors.Red, 2);
        p.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
        g.DrawRectangle(p, drawingContext.DrawingRectangle);
    }
}

public sealed class DrawingContext
{
    public Graphics Graphics { get; private set; }
    public Size Size { get; set; }
    public Point Point { get; set; }
    public ViewType ViewType { get; set; }
    public Rectangle DrawingRectangle => new Rectangle(Point, Size);
}*/