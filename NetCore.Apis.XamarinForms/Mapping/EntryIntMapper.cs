﻿using NetCore.Apis.Client.UI;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NetCore.Apis.XamarinForms.Mapping
{
    public class EntryIntMapper : StackErrorMapper, IComponentMapper<int>
    {
        private readonly Entry entry;

        public EntryIntMapper(Entry entry, StackLayout errorLayout) : base(errorLayout)
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
            if (string.IsNullOrWhiteSpace(entry.Text) || int.TryParse(entry.Text, out int i)) return true;
            errors.Add("Not in the right number format");
            return false;
        }
    }
}

