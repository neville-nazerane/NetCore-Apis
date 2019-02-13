using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Apis.Consumer.InternalModels
{
    class ErrorFormat2_2
    {

        public Dictionary<string, string[]> Errors { get; set; }

        public string Title { get; set; }

        public int Status { get; set; }

        public string TraceId { get; set; }

    }
}
