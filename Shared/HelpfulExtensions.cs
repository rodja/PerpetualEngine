using System.Collections.Generic;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PerpetualEngine
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

        static public string DigitToWord(this string digit)
        {
            switch (digit) {
                case "1":
                    return "eins";
                case "2":
                    return "zwei";
                case "3":
                    return "drei";
                case "4":
                    return "vier";
                case "5":
                    return "fünf";
                case "6":
                    return "sechs";
                case "7":
                    return "sieben";
                case "8":
                    return "acht";
                case "9":
                    return "neun";
                case "10":
                    return "zehn";
                case "11":
                    return "elf";
                case "12":
                    return "zwölf";
                default:
                    return digit;
            }
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