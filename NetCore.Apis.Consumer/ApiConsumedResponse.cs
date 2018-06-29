using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

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
                Errors = DeserializeObject<Dictionary<string, string[]>>(TextResponse);
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
