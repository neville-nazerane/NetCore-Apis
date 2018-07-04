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

    /// <summary>
    /// A wrapper class around HttpResponseMessage with easier access to the response
    /// </summary>
    public class ApiConsumedResponse
    {

        /// <summary>
        /// Basically returns the IsSuccessStatusCode property of the HttpResponseMessage. 
        /// Returns true if the response code is between 200-299
        /// </summary>
        public bool IsSuccessful => Response.IsSuccessStatusCode;

        /// <summary>
        /// Returns true if status code was BadRequest (400)
        /// </summary>
        public bool IsBadRequest => Response.StatusCode == HttpStatusCode.BadRequest;

        /// <summary>
        /// The HttpResponseMessage object that has been wrapped
        /// </summary>
        public HttpResponseMessage Response { get; private set; }

        /// <summary>
        /// List of errors if the status code was 400 and the response was 
        /// serializable into Dictionary<string, string[]>
        /// </summary>
        public Dictionary<string, string[]> Errors { get; set; }
        
        /// <summary>
        /// The response recieved in text format
        /// </summary>
        public string TextResponse { get; }

        /// <summary>
        /// Gets the status code of the HTTP response
        /// </summary>
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
                catch (Exception) { }
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

    /// <summary>
    /// A generic ApiConsumedResponse that serializes the response 
    /// to a generic TModel on success
    /// </summary>
    /// <typeparam name="TModel">The type the response is expected in</typeparam>
    public class ApiConsumedResponse<TModel> : ApiConsumedResponse
    {

        internal ApiConsumedResponse(HttpResponseMessage Response) : base(Response)
        {
        }

        /// <summary>
        /// The serialized response from body
        /// </summary>
        public TModel Data { get; private set; }

        public static implicit operator TModel(ApiConsumedResponse<TModel> model) 
                => model == null ? default(TModel) : model.Data;

        public static implicit operator ApiConsumedResponse<TModel>(HttpResponseMessage response) => new ApiConsumedResponse<TModel>(response);

        internal override void Deserialize(string text) => Data = DeserializeObject<TModel>(TextResponse);

    }

}
