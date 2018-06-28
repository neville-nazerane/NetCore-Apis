using NetCore.Apis.Consumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Apis.Client.UI
{
    public class ModelMapper<TModel>
        where TModel : class, new()
    {

        //private readonly List<IInputMapper> mappers;
        private readonly Dictionary<string,PropertyInfo> properties;
        private readonly List<MappedContext> mappedCollection;

        public ModelMapper()
        {
            //mappers = new List<IInputMapper>();
            properties = typeof(TModel).GetProperties().ToDictionary(p => p.Name);
            mappedCollection = new List<MappedContext>();
        }

        TModel _Model;
        public TModel Model
        {
            get {
                var model = _Model ?? new TModel();
                foreach (var map in mappedCollection) map.Set(model);
                return model;
            }
            set {
                _Model = value;
                foreach (var map in mappedCollection) map.GetFrom(value);
            }
        }

        public void Bind<T>(IInputMapper<T> inputMapper, Expression<Func<TModel, T>> lamda)
        {
            if (lamda.Body is MemberExpression mem)
            {
                var member = mem.Member;
                var info = properties[member.Name];
                mappedCollection.Add(new MappedContext
                {
                    GetFrom = obj => inputMapper.MappedData = (T)info.GetValue(obj),
                    Set = obj => info.SetValue(obj, inputMapper.MappedData)
                });
            }
            else throw new InvalidOperationException("Invalid lamda provided. Property is expected.");
        }

        public async Task<TResult> SubmitAsync<TResult>(
                Func<TModel, Task<ApiConsumedResponse<TResult>>> call,
                Action<TResult> onSuccess,
                Action<Dictionary<string, string[]>> onBadRequest,
                Action<ApiConsumedResponse<TResult>> onError
            )
        {
            var response = await call(Model);
            if (response.Response.IsSuccessStatusCode)
                onSuccess(response);
            else if (response.StatusCode == HttpStatusCode.BadRequest) onBadRequest(response.Errors);
            else onError(response);
            return response;
        }

        public async Task<string> SubmitAsync(
                Func<TModel, Task<ApiConsumedResponse>> call,
                Action<string> onSuccess,
                Action<Dictionary<string, string[]>> onBadRequest,
                Action<ApiConsumedResponse> onError
            )
        {
            var response = await call(Model);
            if (response.Response.IsSuccessStatusCode)
                onSuccess(response.TextResponse);
            else if (response.StatusCode == HttpStatusCode.BadRequest) onBadRequest(response.Errors);
            else onError(response);
            return response.TextResponse;
        }



        class MappedContext
        {
            internal Action<object> Set { get; set; }
            
            internal Action<object> GetFrom { get; set; }

        }

    }
}
