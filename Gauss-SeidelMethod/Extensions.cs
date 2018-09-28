using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gauss_SeidelMethod
{
    internal static class Extensions
    {
        public static void ForEeach<T>(this IEnumerable<T> seq, Action<T> act)
        {
            foreach (var item in seq)
                act(item);
        }

        public static void ForEeach<T>(this IEnumerable<T> seq, Action<T, int> act)
        {
            int index = 0;
            foreach (var item in seq)
                act(item, index++);
        }

    }
}
