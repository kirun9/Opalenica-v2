// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Misc;

using System;
using System.Linq;

using Opalenica.Elements;

public static class EnumerableElementExtensions
{
    public static bool Contains<T>(this IEnumerable<T> enumerable, Guid guid) where T : Element
    {
        return enumerable.Select(e => e.internalGuid).Contains(guid);
    }

    public static bool Contains<T>(this IEnumerable<T> enumerable, T elem) where T : Element
    {
        return enumerable.Select(e => e.internalGuid).Contains(elem.internalGuid);
    }
}
