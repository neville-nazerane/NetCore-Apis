using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Apis.Client.UI
{

    public interface IComponentMapper<T> : IComponentMappper
    {

        T MappedData { get; set; }


    }

    public interface IComponentMappper
    {
        void SetErrors(IEnumerable<string> errors);

        void ClearErrors();

    }

}
