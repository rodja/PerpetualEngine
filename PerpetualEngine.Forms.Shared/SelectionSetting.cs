using System.Collections.Generic;
using System.Linq;

namespace PerpetualEngine.Forms
{

    public class SelectionSetting: Setting
    {
        public IDictionary<string, string> Options;

        public SelectionSetting(string key, string title) : base(key, title)
        {
        }

        public override string Value {
            get {
                return base.Value;
            }
            set {
                if (Options.Count == 0) {
                    base.Value = "";
                    Description.Text = "";
                } else {
                    if (!Options.ContainsKey(value))
                        value = Options.Keys.First();
                    base.Value = value;
                    Description.Text = Options[value];
                }
            }
        }

    }
    
}
