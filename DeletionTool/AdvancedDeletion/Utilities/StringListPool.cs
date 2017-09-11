using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YYProject.AdvancedDeletion
{
    internal static class StringListPool
    {
        private static ConcurrentBag<List<String>> _Objects = new ConcurrentBag<List<String>>();

        public static List<String> Rent()
        {
            if (_Objects.TryTake(out var item))
            {
                return item;
            }
            return new List<String>();
        }

        public static void Return(List<String> ls)
        {
            ls.Clear();
            _Objects.Add(ls);
        }

    }
}