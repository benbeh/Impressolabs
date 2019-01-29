using System;
using System.Collections.Generic;
using System.Linq;

namespace BaseMvvmToolkit.Extensions
{
    public static class GenericCollectionExtension
    {
        public static T SecondLast<T>(this IEnumerable<T> items)
        {
            if (items == null) throw new ArgumentNullException("items");

            if (items is IList<T> list)
            {
                var itemCount = list.Count;

                if (itemCount > 1)
                {
                    return list[itemCount - 2];
                }
                else
                {
                    throw new ArgumentException("Sequence must contain at least two elements.", "items");
                }
            }
            else
            {
                try
                {
                    return items.Reverse().Skip(1).First();
                }
                catch (InvalidOperationException)
                {
                    throw new ArgumentException("Sequence must contain at least two elements.", "items");
                }
            }
        }
    }
}
