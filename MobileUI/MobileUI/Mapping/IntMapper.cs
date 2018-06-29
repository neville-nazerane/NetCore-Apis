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

        public int MappedData {
                get => Int32.Parse(entry.Text);
                set => entry.Text = value.ToString();
        }
    }
}
