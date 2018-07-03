using NetCore.Apis.Client.UI;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NetCore.Apis.XamarinForms.Mapping
{
    public class SwitchBoolMapper : StackErrorMapper, IComponentMapper<bool>
    {
        private readonly Switch component;

        public SwitchBoolMapper(Switch component, StackLayout errorLayout) : base(errorLayout)
        {
            this.component = component;
        }

        public bool MappedData
        {
            get => component.IsToggled;
            set => component.IsToggled = value;
        }

        public bool Validate(List<string> errors) => true;
    }
}
