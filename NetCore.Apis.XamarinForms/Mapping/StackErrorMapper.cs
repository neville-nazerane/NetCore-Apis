using NetCore.Apis.Client.UI;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NetCore.Apis.XamarinForms.Mapping
{
    public abstract class StackErrorMapper : IErrorMapper
    {
        private readonly StackLayout errorStack;

        public StackErrorMapper(StackLayout errorStack)
        {
            this.errorStack = errorStack;
        }

        public void ClearErrors() => errorStack?.Children?.Clear();

        public void SetErrors(IEnumerable<string> errors)
        {
            if (errorStack != null)
                foreach (var err in errors)
                    errorStack.Children.Add(new Label {Text = err, TextColor = Color.Red });
        }

        
    }
}
