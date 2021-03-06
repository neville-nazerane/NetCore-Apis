﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace NetCore.Apis.Consumer.Test
{
    public class TesterAbstract
    {


        public ApiConsumer Consumer { get; }
        internal TestDefaults Defaults { get;}

        public TesterAbstract()
        {
            Consumer = new ApiConsumer(Constants.Urls.TestWebApi)
            {
                Defaults = Defaults = new TestDefaults(),
                ApiVersion = ApiVersion.Version_2_2
            };
        }

    }
}
