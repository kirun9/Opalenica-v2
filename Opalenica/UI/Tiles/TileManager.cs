namespace Opalenica.UI.Tiles;

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
                TileViewManager.GetTileView(location.Key).RegisterTile(tile);
            }
        }
        else
            registeredTiles.Add(tile.Name, tile);
    }
}