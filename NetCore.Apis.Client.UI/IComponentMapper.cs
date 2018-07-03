using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Apis.Client.UI
{

    /// <summary>
    /// Configuration to map a component to a type
    /// </summary>
    /// <typeparam name="T">the type to be mapped</typeparam>
    public interface IComponentMapper<T> : IComponentMapper
    {

        /// <summary>
        /// The data fetched or to be set to the component
        /// </summary>
        T MappedData { get; set; }
        
    }

    public interface IComponentMapper : IErrorMapper
    {

        /// <summary>
        /// Called before a model is created using the mappings. 
        /// This is meant to validate the components contents 
        /// to ensure it has a valid value.
        /// </summary>
        /// <param name="errors">
        /// a list where all errors found can be added. 
        /// These errors will be printed in the UI
        /// </param>
        /// <returns>false if any validation failed</returns>
        bool Validate(List<string> errors);

    }

}
