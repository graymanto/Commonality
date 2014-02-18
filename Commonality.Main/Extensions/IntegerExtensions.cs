using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commonality.Main.Extensions
{
    public static class IntegerExtensions
    {
        public static void Times(this int times, Action<int> func)
        {
            for (int i = 0; i < times; i++)
            {
                func(i);
            }
        }

        public static void Times(this int times, Action func)
        {
            for (int i = 0; i < times; i++)
            {
                func();
            }
        }

        public static void UpTo(this int start, int end, Action<int> func)
        {
            for (int i = start; i <= end; i++)
            {
                func(i);
            }
        }

        public static void UpTo(this int start, int end, Action func)
        {
            for (int i = start; i <= end; i++)
            {
                func();
            }
        }

        public static void DownTo(this int start, int end, Action<int> func)
        {
            for (int i = end; i >= start; i--)
            {
                func(i);
            }
        }

        public static void DownTo(this int start, int end, Action func)
        {
            for (int i = end; i >= start; i--)
            {
                func();
            }
        }

        public static IEnumerable<int> SequenceTo(this int start, int end)
        {
            for (int i = start; i <= end; i++)
            {
                yield return i;
            }
        }
    }
}
