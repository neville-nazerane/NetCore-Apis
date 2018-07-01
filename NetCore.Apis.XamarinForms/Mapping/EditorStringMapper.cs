using NetCore.Apis.Client.UI;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NetCore.Apis.XamarinForms.Mapping
{
    public class EditorStringMapper : StackErrorMapper, IComponentMapper<string>
    {
        private readonly Editor editor;

        public string MappedData
        {
            get => editor.Text;
            set => editor.Text = value;
        }

        public EditorStringMapper(Editor editor, StackLayout errorStack) : base(errorStack)
        {
            this.editor = editor;
        }

        public override bool Validate(List<string> errors) => true;
    }
}
