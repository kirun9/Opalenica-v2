namespace Opalenica.Misc;

using System;
using System.Linq;

public static class EnumarableExtensions
{
    public static T2 FirstOrDefault<T1, T2>(this IEnumerable<KeyValuePair<T1, T2>> pair, Func<T1, T2, bool> predicate)
    {
        return pair.FirstOrDefault(e => predicate.Invoke(e.Key, e.Value)).Value;
    }

    public static T2 FirstOrDefault<T1, T2>(this IEnumerable<KeyValuePair<T1, T2>> pair, Func<T1, T2, bool> predicate, T2 defaultValue)
    {
        return pair.FirstOrDefault(e => predicate.Invoke(e.Key, e.Value), new KeyValuePair<T1, T2>(default, defaultValue)).Value;
    }

    public static T2 FirstOrDefault<T1, T2>(this IEnumerable<KeyValuePair<T1, T2>> pair, T2 defaultValue)
    {
        return pair.FirstOrDefault(new KeyValuePair<T1, T2>(default, defaultValue)).Value;
    }
}