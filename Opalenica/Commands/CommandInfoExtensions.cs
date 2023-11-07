// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Commands;

using Kirun9.CommandParser;

using System;
using System.Collections.Generic;

public static class CommandInfoExtensions
{
    public static bool HasAttribute<T>(this CommandInfo commandInfo)
    {
        foreach (var attribute in commandInfo.Attributes)
        {
            if (attribute is T) return true;
        }
        return false;
    }

    public static T GetAttribute<T>(this CommandInfo commandInfo) where T : Attribute
    {
        foreach (var attribute in commandInfo.Attributes)
        {
            if (attribute is T) return attribute as T;
        }
        return null;
    }

    public static IEnumerable<T> GetAttributes<T>(this CommandInfo commandInfo) where T : Attribute
    {
        foreach (var attribute in commandInfo.Attributes)
        {
            if (attribute is T) yield return attribute as T;
        }
    }
}