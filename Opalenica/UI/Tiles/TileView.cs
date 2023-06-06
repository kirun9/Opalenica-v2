﻿namespace Opalenica.UI.Tiles;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Reflection;

public class TileView
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
}

public class TileViewConverter : JsonConverter<TileView>
{
    public override TileView? ReadJson(JsonReader reader, Type objectType, TileView? existingValue, Boolean hasExistingValue, JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);
        TileView tileView = new TileView(jsonObject[nameof(TileView.ViewID)].Value<string>(), jsonObject[nameof(TileView.Size)].ToObject<Size>());
        tileView.ViewType = serializer.Deserialize<ViewType>(jsonObject[nameof(TileView.ViewType)].CreateReader());
        JArray array = (JArray)jsonObject[nameof(TileView.Tiles)];
        foreach (var tile in array)
        {
            if (tile.Type != JTokenType.Null)
                tileView.AddTile(serializer.Deserialize<Tile>(tile.CreateReader()));
        }
        return tileView;
    }

    public override void WriteJson(JsonWriter writer, TileView? value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName(nameof(TileView.ViewID));
        writer.WriteValue(value.ViewID);
        writer.WritePropertyName(nameof(TileView.Size));
        serializer.Serialize(writer, value.Size);
        writer.WritePropertyName(nameof(TileView.ViewType));
        serializer.Serialize(writer, value.ViewType);
        writer.WritePropertyName(nameof(TileView.Tiles));
        writer.WriteStartArray();
        for (int i = 0; i < value.Tiles.GetLength(0); i++)
        {
            for (int j = 0; j < value.Tiles.GetLength(1); j++)
            {
                var tile = value.Tiles[i, j];
                if (tile is not null and not EmptyTile and not OccupiedTile)
                {
                    if (tile.GetType().GetCustomAttribute<DoNotSaveTileAttribute>() is null)
                        serializer.Serialize(writer, tile);
                }
            }
        }

        writer.WriteEndArray();
        writer.WriteEndObject();
    }
}