// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Graphic;

using Opalenica.Graphic.Exceptions;

public class TileManager
{
    private static Dictionary<string, Tile> registeredTiles = new Dictionary<string, Tile>();
    public static void RegisterTile(Tile tile)
    {
        if (registeredTiles.ContainsKey(tile.Name))
        {
            foreach (var location in tile.Locations)
            {
                if (registeredTiles[tile.Name].Locations.ContainsKey(location.Key))
                    registeredTiles[tile.Name].Locations[location.Key] = location.Value;
                else
                    registeredTiles[tile.Name].Locations.Add(location.Key, location.Value);
                TileViewManager.GetTileView(location.Key).AddTile(tile);
            }
        }
        else
            registeredTiles.Add(tile.Name, tile);
    }

    public static void UnregisterTile(Tile tile, TileView view)
    {
        if (registeredTiles.ContainsKey(tile.Name))
        {
            if (tile.Locations.ContainsKey(view.ViewID))
            {
                tile.Locations.Remove(view.ViewID);
            }
            if (tile.Locations.Count == 0)
            {
                registeredTiles.Remove(tile.Name);
            }
        }
    }

    public static Tile GetTile(string tileName)
    {
        if (registeredTiles.ContainsKey(tileName))
            return registeredTiles[tileName];
        else
            throw new TileNotFoundException(tileName);
    }
}