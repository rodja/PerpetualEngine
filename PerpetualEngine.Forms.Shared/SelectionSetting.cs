using System.Collections.Generic;

namespace PerpetualEngine.Forms
{

    public class SelectionSetting: Setting
    {
        public Dictionary<string, string> Options;

        public SelectionSetting(string title, string defaultValue = "") : base(title, defaultValue)
        {
        }

        public override string Value {
            get {
                return base.Value;
            }
            set {
                if (!Options.ContainsKey(value))
                    value = DefaultValue;
                base.Value = value;
                Description.Text = Options[value];
            }
        }

    }
    
}
