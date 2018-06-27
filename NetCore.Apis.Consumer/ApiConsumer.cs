using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Apis.Consumer
{
    public class ApiConsumer : IDisposable
    {
        private string _bearerToken;
        private string _baseURL;

        public HttpClient Client { get; private set; }

        public string BaseURL
        {
            get => _baseURL;
            set
            {
                _baseURL = value;
                Client.BaseAddress = new Uri(value);
            }
        }

        public string EndingURL { get; set; }

        public string BearerToken
        {
            get => _bearerToken;
            set
            {
                if (value == null)
                    Client.DefaultRequestHeaders.Authorization = null;
                else Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", value);
                _bearerToken = value;
            }
        }

        public ApiConsumer(string BaseURL, long MaxResponseContentBufferSize = 256000)
        {
            Client = new HttpClient
            {
                MaxResponseContentBufferSize = MaxResponseContentBufferSize
            };
            this.BaseURL = BaseURL;
            EndingURL = string.Empty;
        }

        public static implicit operator ApiConsumer(string URL) => new ApiConsumer(URL);
        
        async Task<HttpResponseMessage> DoAsync(Func<string, HttpContent, Task<HttpResponseMessage>> func,
                                                string path, object obj, string BaseURL)
        {
            if (BaseURL == null) BaseURL = this.BaseURL;
            var json = JsonConvert.SerializeObject(obj);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await func(path + EndingURL, content);
            return result;
        }
        
        #region string calls

        public Task<ApiConsumedResponse> PostAsync(string path, object obj, string BaseURL = null)
            => DoStringAsync((u, c) => Client.PostAsync(u, c), path, obj, BaseURL);

        public Task<ApiConsumedResponse> PutAsync(string path, object obj, string BaseURL = null)
            => DoStringAsync((u, c) => Client.PutAsync(u, c), path, obj, BaseURL);

        public Task<ApiConsumedResponse> GetAsync(string path, string BaseURL = null)
            => DoStringAsync((u, c) => Client.GetAsync(u), path, null, BaseURL);

        public Task<ApiConsumedResponse> DeleteAsync(string path, string BaseURL = null)
            => DoStringAsync((u, c) => Client.DeleteAsync(u), path, null, BaseURL);

        async Task<ApiConsumedResponse> DoStringAsync(Func<string, HttpContent, Task<HttpResponseMessage>> func,
                                                string path, object obj, string BaseURL)
                => await DoAsync(func, path, obj, BaseURL);

        #endregion

        #region generic calls

        public Task<ApiConsumedResponse<TModel>> PostAsync<TModel>(string path, object obj, string BaseURL = null)
            => DoAsync<TModel>((u, c) => Client.PostAsync(u, c), path, obj, BaseURL);

        public Task<ApiConsumedResponse<TModel>> PutAsync<TModel>(string path, object obj, string BaseURL = null)
            => DoAsync<TModel>((u, c) => Client.PutAsync(u, c), path, obj, BaseURL);

        public Task<ApiConsumedResponse<TModel>> GetAsync<TModel>(string path, string BaseURL = null)
            => DoAsync<TModel>((u, c) => Client.GetAsync(u), path, null, BaseURL);

        public Task<ApiConsumedResponse<TModel>> DeleteAsync<TModel>(string path, string BaseURL = null)
            => DoAsync<TModel>((u, c) => Client.DeleteAsync(u), path, null, BaseURL);

        async Task<ApiConsumedResponse<TModel>> DoAsync<TModel>(Func<string, HttpContent, Task<HttpResponseMessage>> func,
                                                string path, object obj, string BaseURL)
            => (ApiConsumedResponse<TModel>) await DoAsync(func, path, obj, BaseURL);

        public void Dispose() => Client.Dispose();

        #endregion

    }
}
