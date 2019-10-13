using System;
using System.Collections.Generic;
using System.Linq;

namespace TheMonolith.Simulations
{
    public static class EnumerableEx
    {
        public static IEnumerable<T> TapList<T>(this IEnumerable<T> list, Action<IEnumerable<T>> action)
        {
            action(list);
            return list;
        }
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> list)
        {
            return list.OrderBy(e => Guid.NewGuid());
        }
    }
}
