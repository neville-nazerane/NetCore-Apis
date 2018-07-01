using NetCore.Apis.Client.UI;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NetCore.Apis.XamarinForms.Mapping
{
    public class EntryFloatNullMapper : StackErrorMapper, IComponentMapper<float?>
    {
        private readonly Entry entry;

        public EntryFloatNullMapper(Entry entry, StackLayout errorLayout) : base(errorLayout)
        {
            this.entry = entry;
        }

        public float? MappedData
        {
            get
            {
                if (string.IsNullOrWhiteSpace(entry.Text)) return null;
                return float.Parse(entry.Text);
            }
            set => entry.Text = value.ToString();
        }

        public override bool Validate(List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(entry.Text) || float.TryParse(entry.Text, out float i)) return true;
            errors.Add("Not in the right number format");
            return false;
        }
    }
}

