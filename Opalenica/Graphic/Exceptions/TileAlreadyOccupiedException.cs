// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Graphic.Exceptions;

using System;
using System.Runtime.Serialization;
using Opalenica.Graphic;

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