// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.UI;

using System.Drawing;
using Opalenica.Elements;
using Opalenica.Graphic;
using Opalenica.Graphic.Exceptions;
using Opalenica.Graphic.Interfaces;

public partial class TileView// : ITileView
{
    public string ViewID { get; set; }
    public Size Size { get; set; }
    public Tile[,] Tiles { get; set; }
    public ViewType ViewType { get; set; }

    public TileView(string viewID, Size size)
    {
        ViewID = viewID;
        Size = size;
        Tiles = new Tile[size.Width, size.Height];
        TileViewManager.RegisterView(this);
    }

    public IEnumerable<Tile> GetAllTiles()
    {
        for (int i = 0; i < Tiles.GetLength(0); i++)
            for (int j = 0; j < Tiles.GetLength(1); j++)
                yield return GetTile(new Point(i, j));
    }


    public IEnumerable<Tile> GetTiles()
    {
        for (int i = 0; i < Tiles.GetLength(0); i++)
            for (int j = 0; j < Tiles.GetLength(1); j++)
                if (Tiles[i, j] is not null and not EmptyTile)
                    yield return GetTile(new Point(i, j));
    }

    public Tile GetTile(Point location)
    {
        return Tiles[location.X, location.Y] switch
        {
            OccupiedTile ot => ot.MainTile,
            EmptyTile => new EmptyTile() { Locations = { { ViewID, location } }, ViewID = ViewID },
            null => new EmptyTile() { Locations = { { ViewID, location } }, ViewID = ViewID },
            var tile => tile
        };
    }

    public void RemoveTile(Tile tile)
    {
        var location = tile.Locations[ViewID];
        if (GetTile(location) is not null and not EmptyTile)
        {
            for (int x = 0; x < tile.TileSize.Width; x++)
            {
                for (int y = 0; y < tile.TileSize.Height; y++)
                {
                    if (x == 0 && y == 0) continue;
                    Tiles[location.X + x, location.Y + y] = null;
                }
            }
        }
    }

    public void AddTile<T>(T tile) where T : Tile
    {
        var location = tile.Locations[ViewID];
        if (GetTile(location) is not null and not EmptyTile)
            throw new TileAlreadyOccupiedException(location, tile);
        else
        {
            Tiles[location.X, location.Y] = tile;
            for (int x = 0; x < tile.TileSize.Width; x++)
            {
                for (int y = 0; y < tile.TileSize.Height; y++)
                {
                    if (x == 0 && y == 0) continue;
                    Tiles[location.X + x, location.Y + y] = new OccupiedTile() { MainTile = tile };
                }
            }
        }
    }

    private IEnumerable<Element> SearchElements()
    {
        List<Element> elements = new List<Element>();
        foreach (var tile in Tiles)
        {
            if (tile is IHasElements elementTile)
            {
                var tileElements = elementTile.GetElements();
                foreach (var tileElement in tileElements)
                {
                    if (!elements.Contains(tileElement)) elements.Add(tileElement);
                }
            }
        }
        return elements;
    }
}