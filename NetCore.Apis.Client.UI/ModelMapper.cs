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

        public void Bind<T>(Expression<Func<TModel, T>> lamda, IComponentMapper<T> inputMapper)
        {
            if (lamda.Body is MemberExpression mem)
            {
                var member = mem.Member;
                var info = properties[member.Name];
                mappedCollection.Add(new MappedContext
                {
                    Name = member.Name,
                    GetFrom = obj => inputMapper.MappedData = (T)info.GetValue(obj),
                    Set = obj => info.SetValue(obj, inputMapper.MappedData),
                    Mapper = inputMapper
                });
            }
            else throw new InvalidOperationException("Invalid lamda provided. Property is expected.");
        }

        public async Task<TResult> SubmitAsync<TResult> (
                Func<TModel, Task<ApiConsumedResponse<TResult>>> call,
                Action<TResult> onSuccess = null,
                Action<Dictionary<string, string[]>> onBadRequest = null,
                Action<ApiConsumedResponse<TResult>> onError = null
            ) => await DoSubmitAsync(call, r => onSuccess?.Invoke(r), onBadRequest, onError);

        public async Task<string> SubmitAsync (
                Func<TModel, Task<ApiConsumedResponse>> call,
                Action<string> onSuccess = null,
                Action<Dictionary<string, string[]>> onBadRequest = null,
                Action<ApiConsumedResponse> onError = null
            ) => await DoSubmitAsync(call, r => onSuccess?.Invoke(r), onBadRequest, onError);

        async Task<TResponse> DoSubmitAsync<TResponse>(
                Func<TModel, Task<TResponse>> call,
                Action<TResponse> onSuccess,
                Action<Dictionary<string, string[]>> onBadRequest,
                Action<TResponse> onError
            )
            where TResponse : ApiConsumedResponse
        {
            var response = await call(Model);
            foreach (var map in mappedCollection) map.Mapper.ClearErrors();
            if (response.IsSuccessful) onSuccess?.Invoke(response);
            else if (response.IsBadRequest)
            {
                foreach (var map in mappedCollection)
                    if (response.Errors.ContainsKey(map.Name))
                        map.Mapper.SetErrors(response.Errors[map.Name]);
                onBadRequest?.Invoke(response.Errors);
            }
            else onError?.Invoke(response);
            return response;
        }
        
        class MappedContext
        {
            public string Name { get; set; }

            public Action<object> Set { get; set; }

            public Action<object> GetFrom { get; set; }

            public IComponentMapper Mapper { get; set; }
             
        }

    }
}
