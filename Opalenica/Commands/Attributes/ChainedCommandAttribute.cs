// Copyright (c) PKMK. All rights reserved.

namespace Opalenica.Commands.Attributes;

using System;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class ChainedCommandAttribute : Attribute
{
    public bool IsChained { get; }
    public ChainedCommandAttribute(bool isChained = true)
    {
        IsChained = isChained;
    }
}