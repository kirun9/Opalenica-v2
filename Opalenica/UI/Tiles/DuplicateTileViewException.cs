namespace Opalenica.UI.Tiles;

using System;
using System.Runtime.Serialization;

[Serializable]
public class DuplicateTileViewException : Exception
{
    public DuplicateTileViewException()
    {
    }

    public DuplicateTileViewException(String? message) : base(message)
    {
    }

    public DuplicateTileViewException(String? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected DuplicateTileViewException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}