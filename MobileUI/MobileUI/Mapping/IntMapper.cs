using NetCore.Apis.Client.UI;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MobileUI.Mapping
{
    class IntMapper : StackErrorMapping, IComponentMapper<int>
    {
        private readonly Entry entry;

        public IntMapper(Entry entry, StackLayout errorContainer) 
            : base(errorContainer)
        {
            this.entry = entry;
        }

        public int MappedData
        {
            get
            {
                if (string.IsNullOrWhiteSpace(entry.Text)) return 0;
                return Int32.Parse(entry.Text);
            }
            set => entry.Text = value.ToString();
        }

        public bool Validate(List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(entry.Text) || Int32.TryParse(entry.Text, out int i)) return true;
            errors.Add("Not in the right number format");
            return false;
        }
    }


    class IntNullMapper : StackErrorMapping, IComponentMapper<int?>
    {
        private readonly Entry entry;

        public IntNullMapper(Entry entry, StackLayout errorContainer)
            : base(errorContainer)
        {
            this.entry = entry;
        }

        public int? MappedData
        {
            get
            {
                if (string.IsNullOrWhiteSpace(entry.Text)) return null;
                return int.Parse(entry.Text);
            }
            set => entry.Text = value.ToString();
        }

        public bool Validate(List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(entry.Text) || int.TryParse(entry.Text, out int i)) return true;
            errors.Add("Not in the right number format");
            return false;
        }
    }

}
