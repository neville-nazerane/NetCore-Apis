using NetCore.Apis.Client.UI;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NetCore.Apis.XamarinForms.Mapping
{
    class EntryDoubleNullMapper : StackErrorMapper, IComponentMapper<double?>
    {
        private readonly Entry entry;

        public EntryDoubleNullMapper(Entry entry, StackLayout errorLayout) : base(errorLayout)
        {
            this.entry = entry;
        }

        public double? MappedData
        {
            get
            {
                if (string.IsNullOrWhiteSpace(entry.Text)) return null;
                return double.Parse(entry.Text);
            }
            set => entry.Text = value.ToString();
        }

        public override bool Validate(List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(entry.Text) || double.TryParse(entry.Text, out double d)) return true;
            errors.Add("Not in the right number format");
            return false;
        }
    }
}
