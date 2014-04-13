using System;
using Newtonsoft.Json;
using System.Linq;
using System.Text;

namespace PerpetualEngine
{
    public class Message
    {
        public static StringBuilder LogHistory = null;

        public static void Log(object sender, string message, params object[] messages)
        {
            var items = messages.Select(m => 
            JsonConvert.SerializeObject(m, 
                            Formatting.Indented,
                            new JsonSerializerSettings() { 
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }));

            var prefix = sender is string ? sender : sender.GetType().Name;
            var msg = prefix + ": " + message + (items.Count() > 0 ? "; " : "") + String.Join(", ", items);
            Console.WriteLine(msg);
            if (LogHistory != null)
                LogHistory.AppendLine(DateTime.Now.ToString() + " " + msg);
        }
    }
}
