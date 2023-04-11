namespace Opalenica.UI;

using Kirun9.CommandParser;
using Kirun9.CommandParser.Attributes;

using Newtonsoft.Json;

using Opalenica.Commands;
using Opalenica.UI.Tiles;

using System.Text.RegularExpressions;
using Timer = System.Windows.Forms.Timer;

public class Grid
{
    public static Grid Instance { get; private set; }

    public const string SizeRegex = @"^([1-9][0-9]*)x([1-9][0-9]*)[,]([1-9][0-9]*)x([1-9][0-9]*)$";
    private Timer _timer = new Timer();
    public bool Pulse { get; private set; }
    public Size GridDimensions { get; private set; }
    public Size TileSize { get; set; }
    public bool DebugMode => Program.Settings.DebugMode;
    public Padding Padding { get; set; }

    public TileView CurrentView { get; set; }

    public Grid(string size)
    {
        if (!Regex.IsMatch(size, SizeRegex)) throw new ArgumentException("", nameof(size));
        _timer = new Timer();
        _timer.Interval = 500; // 1Hz, wypełnienie 50/50%
        _timer.Tick += (_, _) => { Pulse = !Pulse; };
        _timer.Enabled = true;

        var sizes = Regex.Split(size, SizeRegex, RegexOptions.IgnoreCase);
        if (sizes.Length is not 6) throw new ArgumentException("", nameof(size));
        GridDimensions = new Size(Int32.Parse(sizes[1]), Int32.Parse(sizes[2]));
        TileSize = new Size(Int32.Parse(sizes[3]), Int32.Parse(sizes[4]));

        Instance = this;
    }

    public IEnumerable<Tile> GetTiles()
    {
        if (CurrentView is null) throw new InvalidOperationException("Cannot get tiles of null view");
        foreach (var tile in CurrentView.GetAllTiles())
        {
            tile.SetContext(CurrentView);
            yield return tile;
        }
    }

    public Point CalculateGraphicTilePosition(Tile tile)
    {
        if (!tile.CheckContext(CurrentView))
        {
            if (tile.Locations.ContainsKey(CurrentView.ViewID))
            {
                tile.SetContext(CurrentView);
            }
            else throw new InvalidOperationException("Cannot calculate position of tile from other view");
        }
        return new Point(tile.Location.X * TileSize.Width + Padding.Left, tile.Location.Y * TileSize.Height + Padding.Top);
    }

    public Tile GetTileFromPoint(Point point)
    {
        var x = (point.X - 2) / TileSize.Width;
        var y = (point.Y - 2) / TileSize.Height;
        return CurrentView.GetTile(new Point(x, y));
    }
}

public class GridCommands : ModuleBase<ICommandContext>
{
    [Command("reload grid")]
    [Alias("rlg")]
    [DebugCommand]
    public void ReloadGrid()
    {
        var grid = Grid.Instance;
        TileViewManager.UnregisterViews();
        TileViewManager.ReadViews();
        //grid.Load(grid.CurrentView);
    }
}