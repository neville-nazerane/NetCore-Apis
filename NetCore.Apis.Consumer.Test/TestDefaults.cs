using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Apis.Consumer.Test
{
    public enum HttpCalled {
        Success, Failed, BadGateway, BadRequest, Unauthorized, Forbidden,
        NotFound, InternalServerError, GatewayTimeout, ServiceUnavailable
    }

    class TestDefaults : ApiConsumerDefaults
    {

        public HttpCalled LastCalled { get; private set; }

        public bool Failed { get; private set; }

        public override Task OnSuccess(ApiConsumedResponse response)
        {
            LastCalled = HttpCalled.Success;
            return base.OnSuccess(response);
        }

        public override Task OnBadGateway(ApiConsumedResponse response)
        {
            LastCalled = HttpCalled.BadGateway;
            return base.OnBadGateway(response);
        }

        public override Task OnFailed(ApiConsumedResponse response)
        {
            LastCalled = HttpCalled.Failed;
            Failed = true;
            return base.OnFailed(response);
        }

        public override Task OnBadRequest(ApiConsumedResponse response)
        {
            LastCalled = HttpCalled.BadRequest;
            return base.OnBadRequest(response);
        }

        public override Task OnForbidden(ApiConsumedResponse response)
        {
            LastCalled = HttpCalled.Forbidden;
            return base.OnForbidden(response);
        }

        public override Task OnGatewayTimeout(ApiConsumedResponse response)
        {
            LastCalled = HttpCalled.GatewayTimeout;
            return base.OnGatewayTimeout(response);
        }

        public override Task OnInternalServerError(ApiConsumedResponse response)
        {
            LastCalled = HttpCalled.InternalServerError;
            return base.OnInternalServerError(response);
        }

        public override Task OnNotFound(ApiConsumedResponse response)
        {
            LastCalled = HttpCalled.NotFound;
            return base.OnNotFound(response);
        }

        public override Task OnServiceUnavailable(ApiConsumedResponse response)
        {
            LastCalled = HttpCalled.ServiceUnavailable;
            return base.OnServiceUnavailable(response);
        }

        public override Task OnUnauthorized(ApiConsumedResponse response)
        {
            LastCalled = HttpCalled.Unauthorized;
            return base.OnUnauthorized(response);
        }
    }
}
