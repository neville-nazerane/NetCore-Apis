using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Apis.Client.UI
{

    public interface IComponentMapper<T> : IComponentMapper
    {

        T MappedData { get; set; }
        
    }

    public interface IComponentMapper
    {

        void SetErrors(IEnumerable<string> errors);

        void ClearErrors();

    }

}
