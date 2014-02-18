using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Commonality.Main.Extensions
{
    public static class DateTimeExtensions
    {
        [DebuggerStepThrough]
        public static DateTime StartOfDay(this DateTime input)
        {
            return input.Date;
        }

        [DebuggerStepThrough]
        public static DateTime EndOfDay(this DateTime input)
        {
            return input.AddDays(1).AddTicks(-1);
        }

        [DebuggerStepThrough]
        public static bool HasDefaultValue(this DateTime input)
        {
            return input == default(DateTime);
        }

        [DebuggerStepThrough]
        public static IEnumerable<DateTime> MakeSequence(this DateTime start, int intervalInSecs)
        {
            var inProgress = start;

            while (true)
            {
                yield return inProgress;
                inProgress = inProgress.AddSeconds(intervalInSecs);
            }
        }
    }
}