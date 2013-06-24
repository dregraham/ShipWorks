using System;
using System.Collections.Generic;


namespace Interapptive.Shared.Collections
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Repeat<T>(this IEnumerable<T> source, int count)
        {
            if (null == source)
                throw new ArgumentNullException("source");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            return RepeatIterator(source, count);
        }

        static IEnumerable<T> RepeatIterator<T>(this IEnumerable<T> source, int count)
        {
            while (0 < count--)
                foreach (var item in source)
                    yield return item;
        }
    }
}
