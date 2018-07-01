using NetCore.Apis.Client.UI;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NetCore.Apis.XamarinForms.Mapping
{
    public class EntryStringMapper : StackErrorMapper, IComponentMapper<string>
    {
        private readonly Entry entry;

        public string MappedData
        {
            get => entry.Text;
            set => entry.Text = value;
        }

        public EntryStringMapper(Entry entry, StackLayout errorStack) : base(errorStack)
        {
            this.entry = entry;
        }

        public override bool Validate(List<string> errors) => true;
    }
}
