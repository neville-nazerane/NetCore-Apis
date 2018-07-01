using NetCore.Apis.Client.UI;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NetCore.Apis.XamarinForms.Mapping
{
    class DateTimeMapper : StackErrorMapper, IComponentMapper<DateTime>
    {
        private readonly DatePicker datePicker;
        private readonly TimePicker timePicker;

        public DateTimeMapper(DatePicker datePicker, TimePicker timePicker, StackLayout errorContainer) : base(errorContainer)
        {
            this.datePicker = datePicker;
            this.timePicker = timePicker;
        }

        public DateTime MappedData
        {
            get => (datePicker?.Date ?? new DateTime()).AddTicks(timePicker?.Time.Ticks ?? 0);
            set
            {
                datePicker.Date = value.Date;
                timePicker.Time = value.TimeOfDay;
            }
        }

        public override bool Validate(List<string> errors) => true;
    }
}
