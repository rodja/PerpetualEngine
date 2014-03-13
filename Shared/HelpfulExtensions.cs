using System.Collections.Generic;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AppBase
{
    public static class HelpfulExtensions
    {
        /// <summary>
        /// implements .ForEach() on IEnumerable
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="action">Action.</param>
        public static void ForEach<T>(
            this IEnumerable<T> source,
            Action<T> action)
        {
            foreach (T element in source)
                action(element);
        }

        public static bool IsEmpty<T>(this List<T> list)
        {
            return list.Count <= 0;
        }

        /// <summary>
        /// Trims the string to the given length and adds three dots (...)
        /// </summary>
        static public string Ellipsis(this string text, int length)
        {
            if (text.Length <= length)
                return text;

            var trimmed = text;
            int pos = text.IndexOf(" ", length);
            if (pos >= 0)
                trimmed = text.Substring(0, pos) + "...";

            if (trimmed.Length <= length)
                return trimmed;

            return trimmed.Substring(0, length - 3) + "...";
        }

        static public T ToEnum<T>(this string enumAsString)
        {
            return (T)Enum.Parse(typeof(T), enumAsString);
        }

        public static TaskAwaiter GetAwaiter(this TimeSpan timeSpan)
        { 
            return Task.Delay(timeSpan).GetAwaiter(); 
        }

        public static void ExecuteWhenFalse(this bool condition, Action action)
        {
            if (!condition)
                action();
        }
    }
}