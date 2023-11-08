// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Graphic.Exceptions;

using System;
using System.Runtime.Serialization;

[Serializable]
public class DuplicateTileViewException : Exception
{
    public DuplicateTileViewException()
    {
    }

    public DuplicateTileViewException(string? message) : base(message)
    {
    }

    public DuplicateTileViewException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected DuplicateTileViewException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}