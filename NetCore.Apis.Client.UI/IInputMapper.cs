using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Apis.Client.UI
{

    public interface IInputMapper<T>
    {

        T MappedData { get; set; }


    }

    //public interface IInputMapper<TComponent> : IInputMapper
    //{

    //    TComponent Component { get; }
        
    //}

}
