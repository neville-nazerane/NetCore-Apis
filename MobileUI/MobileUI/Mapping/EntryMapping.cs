using NetCore.Apis.Client.UI;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MobileUI.Mapping
{
    class EntryMapping : IComponentMapper<string>
    {
        private readonly Entry entry;
        private readonly StackLayout errorContainer;

        public string MappedData
        {
            get => entry.Text;
            set => entry.Text = value;
        }

        public EntryMapping(Entry entry, StackLayout errorContainer)
        {
            this.entry = entry;
            this.errorContainer = errorContainer;
        }

        public void SetErrors(IEnumerable<string> errors)
        {
            foreach (var error in errors)
                errorContainer.Children.Add(new Label {
                    TextColor = Color.Red,
                    Text = error
                });
        }

        public void ClearErrors() => errorContainer.Children.Clear();
    }
}
