using System;
using System.Collections.Generic;

namespace PerpetualEngine
{
    public static class PerformanceTimer
    {
        static Dictionary<string, DateTime> times = new Dictionary<string, DateTime>();

        static string latestUsedLabel;

        /// <summary>
        /// Start a performance measurement with the specified label.
        /// </summary>
        /// <param name="label">Label.</param>
        public static void Start(string label)
        {
            times[label] = DateTime.Now;
            latestUsedLabel = label;
        }

        /// <summary>
        /// Stop and print performance measurement with the label used in "Start" command.
        /// </summary>
        public static void Stop(string label)
        {
            if (times.ContainsKey(label))
                Console.WriteLine(label + " " + (DateTime.Now - times[label]).TotalMilliseconds + " ms");
        }

        /// <summary>
        /// Stop and print performance measurement of last used "Start" command.
        /// </summary>
        public static void Stop()
        {
            Stop(latestUsedLabel);
        }
    }
}

