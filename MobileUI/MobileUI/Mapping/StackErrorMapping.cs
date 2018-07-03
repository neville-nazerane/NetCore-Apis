using NetCore.Apis.Client.UI;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MobileUI.Mapping
{
    class StackErrorMapping : IErrorMapper
    {
        private readonly StackLayout errorContainer;

        public StackErrorMapping(StackLayout errorContainer)
        {
            this.errorContainer = errorContainer;
        }

        public void SetErrors(IEnumerable<string> errors)
        {
            foreach (var error in errors)
                errorContainer.Children.Add(new Label
                {
                    TextColor = Color.Red,
                    Text = error
                });
        }

        public void ClearErrors() => errorContainer.Children.Clear();
        
    }
}
