namespace Opalenica.UI.Tiles;

using System;
using System.Runtime.Serialization;

[Serializable]
public class TileViewNotFoundException : Exception
{
    public TileViewNotFoundException()
    {
    }

    public TileViewNotFoundException(String? message) : base(message)
    {
    }

    public TileViewNotFoundException(String? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected TileViewNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}