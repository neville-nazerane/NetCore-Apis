using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Apis.Consumer
{

    /// <summary>
    /// A wrapper around HttpClient, designed to simplify consuming an API
    /// </summary>
    public class ApiConsumer : IDisposable
    {
        private string _bearerToken;
        private string _baseURL;

        /// <summary>
        /// The default functionality assigned to this consumer
        /// </summary>
        public ApiConsumerDefaults Defaults { get; set; }

        /// <summary>
        /// The HttpClient the current object is wrapped around.
        /// This can be used to access advanced features not 
        /// currently present in ApiConsumer
        /// </summary>
        public HttpClient Client { get; private set; }

        /// <summary>
        /// The version of .net core API being consumed. Needs to be 
        /// the highest value that is closest to the API version.
        /// If the API is not .net core, None can be selected.
        /// If nothing is selected, every version will be tried (will be a lot heavier).
        /// </summary>
        public ApiVersion ApiVersion { get; set; }

        /// <summary>
        /// The base url of the REST API 
        /// </summary>
        public string BaseURL
        {
            get => _baseURL;
            set
            {
                _baseURL = value;
                Client.BaseAddress = new Uri(value);
            }
        }

        /// <summary>
        /// A default ending url. This can be used to assign any URL parameters that need to be 
        /// passed on every request
        /// </summary>
        public string EndingURL { get; set; }

        /// <summary>
        /// A JWT token to be set as the authentication header
        /// </summary>
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

        /// <summary>
        /// Creates a new instance of ApiConsumer, also creating a new instance of HttpClient
        /// </summary>
        /// <param name="BaseURL">The base url of the api
        /// </param>
        /// <param name="MaxResponseContentBufferSize">
        /// the maximum number of bytes to buffer when reading the response content
        /// </param>
        public ApiConsumer(string BaseURL, long MaxResponseContentBufferSize = 256000)
            : this(new HttpClient
            {
                MaxResponseContentBufferSize = MaxResponseContentBufferSize,
                BaseAddress = new Uri(BaseURL)
            })
        { }

        public ApiConsumer(HttpClient client)
        {
            Client = client;
            _baseURL = client.BaseAddress.AbsoluteUri;
            EndingURL = string.Empty;
        }

        public static implicit operator ApiConsumer(string URL) => new ApiConsumer(URL);
        
        async Task<HttpResponseMessage> DoAsync(Func<string, HttpContent, Task<HttpResponseMessage>> func,
                                                string path, object obj, string BaseURL)
        {
            var json = JsonConvert.SerializeObject(obj);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await func(path + EndingURL, content);
            return result;
        }
        
        #region string calls


        public async Task<ApiConsumedResponseProvider> PostAsync(string path, object obj, string BaseURL = null)
            => await DoStringAsync((u, c) => Client.PostAsync(u, c), path, obj, BaseURL);

        public async Task<ApiConsumedResponseProvider> PutAsync(string path, object obj, string BaseURL = null)
            => await DoStringAsync((u, c) => Client.PutAsync(u, c), path, obj, BaseURL);

        public async Task<ApiConsumedResponseProvider> GetAsync(string path, string BaseURL = null)
            => await DoStringAsync((u, c) => Client.GetAsync(u), path, null, BaseURL);

        public async Task<ApiConsumedResponseProvider> DeleteAsync(string path, string BaseURL = null)
            => await DoStringAsync((u, c) => Client.DeleteAsync(u), path, null, BaseURL);

        async Task<ApiConsumedResponseProvider> DoStringAsync(Func<string, HttpContent, Task<HttpResponseMessage>> func,
                                                string path, object obj, string BaseURL)
        {
            var res = new ApiConsumedResponseProvider(await DoAsync(func, path, obj, BaseURL), ApiVersion);
            await res.RunDefaults(Defaults);
            return res;
        }

        #endregion

        #region build calls 

        public async Task<ApiConsumedResponse> PostBuiltAsync(string path, object obj, string BaseURL = null)
            => await DoStringAsync((u, c) => Client.PostAsync(u, c), path, obj, BaseURL);

        public async Task<ApiConsumedResponse> PutBuiltAsync(string path, object obj, string BaseURL = null)
            => await DoStringAsync((u, c) => Client.PutAsync(u, c), path, obj, BaseURL);

        public async Task<ApiConsumedResponse> GetBuiltAsync(string path, string BaseURL = null)
            => await DoStringAsync((u, c) => Client.GetAsync(u), path, null, BaseURL);

        public async Task<ApiConsumedResponse> DeleteBuiltAsync(string path, string BaseURL = null)
            => await DoStringAsync((u, c) => Client.DeleteAsync(u), path, null, BaseURL);

        #endregion

        #region generic calls

        async Task<ApiConsumedResponse<TModel>> DoAsync<TModel>(Func<string, HttpContent, Task<HttpResponseMessage>> func,
                                        string path, object obj, string BaseURL)
        {
            var res = new ApiConsumedResponseProvider(await DoAsync(func, path, obj, BaseURL), ApiVersion);
            await res.RunDefaults(Defaults);
            return res;
        }

        public Task<ApiConsumedResponse<TModel>> PostAsync<TModel>(string path, object obj, string BaseURL = null)
            => DoAsync<TModel>((u, c) => Client.PostAsync(u, c), path, obj, BaseURL);

        public Task<ApiConsumedResponse<TModel>> PutAsync<TModel>(string path, object obj, string BaseURL = null)
            => DoAsync<TModel>((u, c) => Client.PutAsync(u, c), path, obj, BaseURL);

        public Task<ApiConsumedResponse<TModel>> GetAsync<TModel>(string path, string BaseURL = null)
            => DoAsync<TModel>((u, c) => Client.GetAsync(u), path, null, BaseURL);

        public Task<ApiConsumedResponse<TModel>> DeleteAsync<TModel>(string path, string BaseURL = null)
            => DoAsync<TModel>((u, c) => Client.DeleteAsync(u), path, null, BaseURL);

        /// <summary>
        /// Disposes the HttpClient object
        /// </summary>
        public void Dispose() => Client.Dispose();

        #endregion

    }
}
