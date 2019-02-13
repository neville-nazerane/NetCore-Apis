using NetCore.Apis.Consumer.InternalModels;
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
        /// The version of .net core API being consumed. Needs to be 
        /// the highest value that is closest to the API version.
        /// If the API is not .net core, None can be selected.
        /// If nothing is selected, every version will be tried (will be a lot heavier).
        /// </summary>
        public ApiVersion ApiVersion { get; set; }

        /// <summary>
        /// List of errors if the status code was 400 and the response was 
        /// serializable into Dictionary<string, string[]>
        /// </summary>
        public Dictionary<string, string[]> Errors { get; set; }

        public ErrorMeta ErrorMeta { get; internal set; }

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
                switch (ApiVersion)
                {
                    case ApiVersion.None:
                        break;
                    case ApiVersion.Version_2_1:
                        FormatToError2_1(TextResponse);
                        break;
                    case ApiVersion.Version_2_2:
                        FormatToError2_2(TextResponse);
                        break;
                    case ApiVersion.Default:
                        try
                        {
                            FormatToError2_2(TextResponse);
                        }
                        catch
                        {
                            try
                            {
                                FormatToError2_1(TextResponse);
                            }
                            catch { }
                        }
                        break;
                }
            }
        }

        void FormatToError2_2(string text)
        {
            var values = DeserializeObject<ErrorFormat2_2>(text);
            Errors = values.Errors;
            ErrorMeta = new ErrorMeta
            {
                Title = values.Title,
                TraceId = values.TraceId
            };
        }

        void FormatToError2_1(string text)
        {
            Errors = DeserializeObject<Dictionary<string, string[]>>(text);
        }

        public static implicit operator ApiConsumedResponse(HttpResponseMessage response) => new ApiConsumedResponse(response);

        public static implicit operator ApiConsumedResponse(ApiConsumedResponseProvider provider)
        {
            ApiConsumedResponse res = provider.response;
            res.ApiVersion = provider.ApiVersion;
            return res;
        }

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

        public static implicit operator ApiConsumedResponse<TModel>(ApiConsumedResponseProvider provider)
        {
            ApiConsumedResponse<TModel>  res = provider.response;
            res.ApiVersion = provider.ApiVersion;
            return res;
        }

        internal override void Deserialize(string text) => Data = DeserializeObject<TModel>(TextResponse);

    }

}
