using System;

namespace Commonality.Main.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfDay(this DateTime input)
        {
            return new DateTime(input.Year, input.Month, input.Day, 00, 00, 00);
        }

        public static DateTime EndOfDay(this DateTime input)
        {
            return new DateTime(input.Year, input.Month, input.Day, 23, 59, 59);
        }
    }
}
