using System;
using System.Collections.Generic;

namespace Quoridor
{
    public static class Helpers
    {
        public static Random Random = new Random();

        public static T PickRandomElement<T>(IList<T> list)
        {
            return list[Random.Next(list.Count)];
        }
    }
}