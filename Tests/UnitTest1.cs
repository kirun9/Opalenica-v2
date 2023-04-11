namespace Tests;

using NUnit.Framework;

using Opalenica;
using Opalenica.UI;
using Opalenica.UI.Tiles;

using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

public class Tests
{
    [Test]
    [TestOf(nameof(Grid.SizeRegex))]
    [Order(1)]
    public void Grid_SizeRegex()
    {
        Assert.IsTrue(Regex.IsMatch("1x1,1x1", Grid.SizeRegex));
        Assert.IsTrue(Regex.IsMatch("10x10,10x10", Grid.SizeRegex));
        Assert.IsTrue(Regex.IsMatch("1x100,100x100", Grid.SizeRegex));
        Assert.IsTrue(Regex.IsMatch("1000x1000,1000x1000", Grid.SizeRegex));
        Assert.IsTrue(Regex.IsMatch("10000x10000,10000x10000", Grid.SizeRegex));
        Assert.IsTrue(Regex.IsMatch("100000x100000,1x1", Grid.SizeRegex));
        Assert.IsFalse(Regex.IsMatch("0x0,0x0", Grid.SizeRegex));
        Assert.IsFalse(Regex.IsMatch("0x0,1x1", Grid.SizeRegex));
        Assert.IsFalse(Regex.IsMatch("1x1,0x0", Grid.SizeRegex));
        Assert.IsFalse(Regex.IsMatch("1x1,1x1,1x1", Grid.SizeRegex));
        Assert.IsFalse(Regex.IsMatch("1x0,1x1", Grid.SizeRegex));
    }

    [SetUp]
    public void UnregisterEveryTileAndView()
    {
        TileViewManager.UnregisterViews();
        if (Directory.Exists("Views"))
            Directory.Delete("Views", true);
    }


    [Test]
    [TestOf(nameof(TileViewManager.UnregisterView))]
    public void TileTests()
    {
        TileView view1 = new TileView("Test1", new Size(10, 10));
        TileView view2 = new TileView("Test2", new Size(10, 10));
        TileBuilder b = new TileBuilder();
        b.WithName("TestTile_1");
        b.WithLocation(view1, new Point(1, 1));
        b.WithLocation(view2, new Point(2, 2));
        b.BuildTile();
        var c = view1.GetTiles().Where(e => e is not null).Count();
        Assert.IsTrue(c == 1);
        var tile = TileManager.GetTile("TestTile_1");
        Assert.IsTrue(tile.Locations.Count == 2);
        TileViewManager.UnregisterView(view1);
        Assert.IsTrue(tile.Locations.Count == 1);
    }

    [Test]
    [TestOf(nameof(TileViewManager.SaveViews))]
    [TestOf(nameof(TileViewManager.ReadViews))]
    public void SaveReadTileTest()
    {
        TileView view1 = new TileView("Test1", new Size(2, 2));
        TileBuilder b = new TileBuilder();
        b.WithName("TestTile_1");
        b.WithLocation(view1, new Point(1, 1));
        b.BuildTile();
        ExtendedTile tile = new ExtendedTile();
        tile.Name = "TestTile_2";
        tile.Locations.Add(view1.ViewID, new Point(0, 0));
        tile.MoreData = "MoreData";
        view1.AddTile(tile);
        TileViewManager.SaveViews();
        TileViewManager.UnregisterView(view1);
        Assert.Throws(typeof(TileNotFoundException), () => TileManager.GetTile("TestTile_1"));
        Assert.Throws(typeof(TileViewNotFoundException), () => TileViewManager.GetTileView("Test1"));
        TileViewManager.ReadViews();
        Assert.DoesNotThrow(() => TileViewManager.GetTileView("Test1"));
        Assert.DoesNotThrow(() => TileManager.GetTile("TestTile_1"));
        Assert.DoesNotThrow(() => TileManager.GetTile("TestTile_2"));
        Assert.IsTrue(TileManager.GetTile("TestTile_2") is ExtendedTile);
        Assert.IsTrue(TileManager.GetTile("TestTile_2") is ExtendedTile and { MoreData: "MoreData" });
    }

    [Test]
    [TestOf(nameof(TileViewSerializer.Serialize))]
    [TestOf(nameof(TileViewSerializer.Deserialize))]
    public void SerializeDeserializeTileView_Success()
    {
        // Arrange
        var viewID = "test_view";
        var size = new Size(2, 2);
        var view = new TileView(viewID, size);
        var tile1 = new TileBuilder().WithName("tile1").WithLocation(view, new Point(0, 0)).BuildTile();
        var tile2 = new TileBuilder().WithName("tile2").WithLocation(view, new Point(1, 1)).BuildTile();

        // Act
        var serialized = TileViewSerializer.Serialize(view);
        TileViewManager.UnregisterView(view);
        var deserialized = TileViewSerializer.Deserialize(serialized);

        // Assert
        Assert.IsNotNull(deserialized);
        Assert.AreEqual(viewID, deserialized.ViewID);
        Assert.AreEqual(size, deserialized.Size);
        Assert.IsNotNull(deserialized.Tiles);
        Assert.AreEqual(tile1.Name, deserialized.Tiles[0, 0].Name);
        Assert.AreEqual(tile2.Name, deserialized.Tiles[1, 1].Name);
    }

    [Test]
    public void GridTest()
    {
        Grid grid = new Grid("34x19,40x40");
        new TileView("debugView", grid.GridDimensions);
        grid.CurrentView = TileViewManager.GetTileView("debugView");
        Assert.AreEqual(grid.GridDimensions.Width * grid.GridDimensions.Height, grid.GetTiles().Count());
    }
}