namespace Opalenica.UI.Tiles;

using System;
using System.Runtime.Serialization;

[Serializable]
public class TileNotFoundException : Exception
{
    public TileNotFoundException()
    {
    }

    public TileNotFoundException(String? message) : base(message)
    {
    }

    public TileNotFoundException(String? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected TileNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}