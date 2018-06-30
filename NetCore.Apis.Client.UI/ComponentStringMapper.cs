using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NetCore.Apis.Client.UI
{
    public abstract class ComponentStringMapper<T> : IComponentMapper<T>
    {

        protected abstract string Data { get; set; }

        public virtual T MappedData
        {
            get => (T) converter.ConvertFromString(Data);
            set => Data.ToString();
        }

        readonly TypeConverter converter;

        public ComponentStringMapper()
        {
            converter = TypeDescriptor.GetConverter(typeof(T));
        }

        public abstract void ClearErrors();
        public abstract void SetErrors(IEnumerable<string> errors);

        public virtual bool Validate(List<string> errors)
        {
            try
            {
                converter.ConvertFromString(Data);
            }
            catch (Exception)
            {
                errors.Add("Invalid format");
                return false;
            }
            return true;
        }
    }
}
