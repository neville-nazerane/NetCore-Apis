using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace NetCore.Apis.Consumer.Test
{
    public class TesterAbstract
    {

        public ApiConsumer Consumer { get; }

        public TesterAbstract()
        {
            Consumer = new ApiConsumer(Constants.Urls.TestWebApi);
        }

    }
}
