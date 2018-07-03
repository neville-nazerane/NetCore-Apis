using NetCore.Apis.Client.UI;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MobileUI.Mapping
{
    class EntryMapping : StackErrorMapping, IComponentMapper<string>
    {
        private readonly Entry entry;

        public string MappedData
        {
            get => entry.Text;
            set => entry.Text = value;
        }

        public EntryMapping(Entry entry, StackLayout errorContainer)
            : base(errorContainer)
        {
            this.entry = entry;
        }

        public bool Validate(List<string> errors) => true;
    }
}
