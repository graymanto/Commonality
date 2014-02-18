using System;
using System.Collections.Generic;
using System.Linq;

namespace Commonality.Main.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Ascertains if a string is populated.
        /// </summary>
        /// <param name="check">The string to test</param>
        /// <returns></returns>
        public static bool IsPopulated(this string check)
        {
            return !string.IsNullOrEmpty(check);
        }

        /// <summary>
        /// Ascertains if a string is null or empty.
        /// </summary>
        /// <param name="input">The string to test</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        /// <summary>
        /// Implements String.Format() as an extension method.
        /// </summary>
        /// <param name="expression">A composite format string</param>
        /// <param name="args">An object array containing zero or more objects to format</param>
        /// <returns></returns>
        public static string Formatted(this string expression, params object[] args)
        {
            if (expression.IsNullOrEmpty())
            {
                return expression;
            }
            return string.Format(expression, args);
        }

        /// <summary>
        /// Tries to parse the supplied string to an integer or provides a default.
        /// </summary>
        /// <param name="input">The string to parse.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static int TryParseInt(this string input, int defaultValue)
        {
            int result;
            return int.TryParse(input, out result) ? result : defaultValue;
        }

        /// <summary>
        /// Parse the supplied string into an integer.
        /// </summary>
        /// <param name="input">The string to parse.</param>
        /// <returns></returns>
        public static int TryParseInt(this string input)
        {
            return TryParseInt(input, 0);
        }

        public static Guid TryParseGuid(this string input)
        {
            Guid result;
            return Guid.TryParse(input, out result) ? result : Guid.Empty;

        }

        public static string IfNullOrEmpty(this string a, string b)
        {
            return a.IsNullOrEmpty() ? b : a;
        }

    }
}
