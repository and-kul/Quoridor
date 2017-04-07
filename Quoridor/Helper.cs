using System;
using System.Collections.Generic;
using System.Linq;

namespace Quoridor
{
    public static class Helper
    {
        public static void Swap<T>(ref T left, ref T right)
        {
            var tmp = left;
            left = right;
            right = tmp;
        }

        public static Random Random = new Random();

        public static T PickRandomElement<T>(IList<T> list)
        {
            return list[Random.Next(list.Count)];
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

    }
}