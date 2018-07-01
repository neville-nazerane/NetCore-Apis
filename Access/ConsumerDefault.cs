using Constants;
using NetCore.Apis.Consumer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access
{
    public class ConsumerDefault
    {

        static ApiConsumer _Consumer;
        public static ApiConsumer Consumer => _Consumer ?? (_Consumer = new ApiConsumer(Urls.TestWebApi));

    }
}
