using System;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using Android.Widget;
using Android.Content;

namespace PerpetualEngine
{
    public partial class Msg
    {
        static Context context;

        public static void SetContext(Context c)
        {
            context = c;
        }

        public static void Show(string message, params object[] messages)
        {
            var items = messages.Select(m => 
            JsonConvert.SerializeObject(m, 
                            Formatting.Indented,
                            new JsonSerializerSettings() { 
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }));

            var msg = message + (items.Count() > 0 ? "; " : "") + String.Join(", ", items);
            Toast.MakeText(context, msg, ToastLength.Short).Show();
            if (LogHistory != null)
                LogHistory.AppendLine(DateTime.Now.ToString() + " " + msg);
        }
    }
}
