namespace Opalenica.UI.Tiles;

using System;
using System.Drawing;
using System.Runtime.Serialization;

[Serializable]
public class TileAlreadyOccupiedException : Exception
{
    private Point location;
    private Tile tile;

    public TileAlreadyOccupiedException()
    {
    }

    public TileAlreadyOccupiedException(String? message) : base(message)
    {
    }

    public TileAlreadyOccupiedException(Point location, Tile tile)
    {
        this.location = location;
        this.tile = tile;
    }

    public TileAlreadyOccupiedException(String? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected TileAlreadyOccupiedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}