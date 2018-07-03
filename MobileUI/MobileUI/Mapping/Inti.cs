using NetCore.Apis.Client.UI;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MobileUI.Mapping
{
    class Inti : ComponentStringMapper<int>
    {
        private readonly Entry entry;
        private readonly StackLayout layout;
        private readonly StackErrorMapping errMapping;

        protected override string Data
        {
            get => entry.Text;
            set => entry.Text = value;
        }

        public Inti(Entry entry, StackLayout layout)
        {
            this.entry = entry;
            this.layout = layout;
            errMapping = new Err(layout);
        }

        public override void ClearErrors() => errMapping.ClearErrors();

        public override void SetErrors(IEnumerable<string> errors) => errMapping.SetErrors(errors);

        class Err : StackErrorMapping
        {
            public Err(StackLayout errorContainer) : base(errorContainer)
            {
            }

            public bool Validate(List<string> errors) => true;
        }

    }

}
