using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Newtonsoft.Json.JsonConvert;

namespace NetCore.Apis.Consumer
{
    public class ApiConsumedResponse
    {

        public bool IsSuccessful => Response.IsSuccessStatusCode;

        public bool IsBadRequest => Response.StatusCode == HttpStatusCode.BadRequest;

        public HttpResponseMessage Response { get; private set; }

        public Dictionary<string, string[]> Errors { get; set; }
        
        public string TextResponse { get; }

        public HttpStatusCode StatusCode => Response.StatusCode;

        internal ApiConsumedResponse(HttpResponseMessage Response)
        {

            this.Response = Response;
            var jsonTask = Response.Content.ReadAsStringAsync();
            if (!jsonTask.IsCompleted) jsonTask.RunSynchronously();
            TextResponse = jsonTask.Result;
            if (string.IsNullOrWhiteSpace(TextResponse)) return;
            if (StatusCode == HttpStatusCode.OK) Deserialize(TextResponse);
            else if (StatusCode == HttpStatusCode.BadRequest)
            {
                try
                {
                    Errors = DeserializeObject<Dictionary<string, string[]>>(TextResponse);
                }
                catch (JsonReaderException) { }
            }
        }

        internal async Task RunDefaults(ApiConsumerDefaults defaults)
        {
            if (defaults != null)
            {
                if (IsSuccessful) await defaults.OnSuccess(this);
                else
                {
                    await defaults.OnFailed(this);
                    switch (StatusCode)
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

        public static implicit operator ApiConsumedResponse(HttpResponseMessage response)
            => new ApiConsumedResponse(response);

        public static implicit operator string(ApiConsumedResponse response) => response?.TextResponse;

        internal virtual void Deserialize(string text) { }

    }

    public class ApiConsumedResponse<TModel> : ApiConsumedResponse
    {

        internal ApiConsumedResponse(HttpResponseMessage Response) : base(Response)
        {
        }

        public TModel Data { get; private set; }

        public static implicit operator TModel(ApiConsumedResponse<TModel> model) 
                => model == null ? default(TModel) : model.Data;

        public static implicit operator ApiConsumedResponse<TModel>(HttpResponseMessage response) => new ApiConsumedResponse<TModel>(response);

        internal override void Deserialize(string text) => Data = DeserializeObject<TModel>(TextResponse);

    }

}
