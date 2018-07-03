using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Apis.Client.UI
{
    public interface IErrorMapper
    {

        /// <summary>
        /// Sets the given list of errors that belong to the current component.
        /// This method is meant to set the errors into the required UI component
        /// </summary>
        /// <param name="errors">the collection of errors to be displayed</param>
        void SetErrors(IEnumerable<string> errors);

        /// <summary>
        /// Removes all errors from the current component
        /// </summary>
        void ClearErrors();


    }
}
