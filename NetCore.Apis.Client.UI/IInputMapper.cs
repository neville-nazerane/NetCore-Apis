using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Apis.Client.UI
{

    public interface IInputMapper
    {

        object MappedData { get; set; }


    }

    public interface IInputMapper<TComponent> : IInputMapper
    {

        TComponent Component { get; }
        
    }

}
