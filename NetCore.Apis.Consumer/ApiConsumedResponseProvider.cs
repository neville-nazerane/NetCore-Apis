using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Apis.Consumer
{
    /// <summary>
    /// Implicit converter to ApiConsumedResponse and ApiConsumedResponse<>
    /// </summary>
    public class ApiConsumedResponseProvider
    {
        internal readonly HttpResponseMessage response;

        internal ApiConsumedResponseProvider(HttpResponseMessage Response)
        {
            response = Response;
        }

        public ApiConsumedResponse Build() => this;

        public ApiConsumedResponse<TModel> Build<TModel>() => this;

        internal async Task RunDefaults(ApiConsumerDefaults defaults)
        {
            if (defaults != null)
            {
                if (response.IsSuccessStatusCode) await defaults.OnSuccess(this);
                else
                {
                    await defaults.OnFailed(this);
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.BadRequest:
                            await defaults.OnBadRequest(this);
                            break;
                        case HttpStatusCode.Forbidden:
                            await defaults.OnForbidden(this);
                            break;
                        case HttpStatusCode.NotFound:
                            await defaults.OnNotFound(this);
                            break;
                        case HttpStatusCode.InternalServerError:
                            await defaults.OnInternalServerError(this);
                            break;
                        case HttpStatusCode.BadGateway:
                            await defaults.OnBadGateway(this);
                            break;
                        case HttpStatusCode.GatewayTimeout:
                            await defaults.OnGatewayTimeout(this);
                            break;
                        case HttpStatusCode.ServiceUnavailable:
                            await defaults.OnServiceUnavailable(this);
                            break;
                        case HttpStatusCode.Unauthorized:
                            await defaults.OnUnauthorized(this);
                            break;
                    }
                }
            }
        }

    }
}
