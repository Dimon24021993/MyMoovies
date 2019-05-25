using System.Collections.Generic;
using System.Linq;

namespace MyMovies.BLL.Extensions
{
    public static class IEnumerables
    {
        /**
         * Keeps removing every other item, except min and max, until 5 remain,
         * so user filters lots by existing options with enough results.
         */
        public static IEnumerable<T> Range<T>(this IEnumerable<T> values, int limit = 5)
        {
            //var limit = 5;

            var xs = values.ToList();
            xs.Sort();

            while (xs.Count() > limit)
            {
                for (var i = xs.Count() - 2; xs.Count() > limit && i >= 1; i -= 2)
                {
                    xs.RemoveAt(i);
                }
            }

            return xs;
        }
    }
}
