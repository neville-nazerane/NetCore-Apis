using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NetCore.Apis.Client.UI
{

    /// <summary>
    /// This class is meant to be a helper for creating component mappers.
    /// Considering most UI components allow their contents to be accessed 
    /// as strings, this class handles the conversion between strings and 
    /// a generic type. It also handles the required validations for the same. 
    /// However, for better performance, it is recommended to directly implement
    /// IComponentMapper, since this class uses reflection. 
    /// </summary>
    /// <typeparam name="T">The generic type being mapped</typeparam>
    public abstract class ComponentStringMapper<T> : IComponentMapper<T>
    {

        /// <summary>
        /// The data that requires to be mapped with the UI component
        /// </summary>
        protected abstract string Data { get; set; }

        /// <summary>
        /// Maps Data to the type T
        /// </summary>
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
