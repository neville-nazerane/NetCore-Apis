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

        private readonly Dictionary<string, PropertyInfo> properties;
        private readonly List<MappedContext> mappedCollection;

        public ModelMapper()
        {
            properties = typeof(TModel).GetProperties().ToDictionary(p => p.Name);
            mappedCollection = new List<MappedContext>();
            _Model = new TModel();
        }

        TModel _Model;
        public void SetModel(TModel value)
        {
            _Model = value;
            foreach (var map in mappedCollection) map.GetFrom(value);
        }

        public void ClearErrors()
        {
            foreach (var map in mappedCollection) map.Mapper.ClearErrors();
        }

        /// <summary>
        /// 
        /// Tries to get all properties of the model from the UI.
        /// If fails, prints the errors on the UI.
        /// 
        /// Note that even on fail, all successful properties will 
        /// stil be returned by the model object.
        /// 
        /// </summary>
        /// <param name="model">Model object with all properties without errors</param>
        /// <returns>true if all validations passed</returns>
        public bool TryGetModel(out TModel model, bool displayErrors = true)
        {
            bool isValid = true;
            model = _Model;
            foreach (var map in mappedCollection)
            {
                var errors = new List<string>();
                if (map.Mapper.Validate(errors)) map.Set(model);
                else
                {
                    if (displayErrors)
                    {
                        map.Mapper.ClearErrors();
                        map.Mapper.SetErrors(errors);
                    }
                    isValid = false;
                }
            }
            return isValid;
        }

        public ModelMapper<TModel> Bind<T>(Expression<Func<TModel, T>> lamda, IComponentMapper<T> inputMapper)
        {
            if (lamda.Body is MemberExpression mem)
            {
                var member = mem.Member;
                var info = properties[member.Name];
                mappedCollection.Add(new MappedContext
                {
                    Name = member.Name,
                    GetFrom = obj => inputMapper.MappedData = (T) info.GetValue(obj),
                    Set = obj => info.SetValue(obj, inputMapper.MappedData),
                    Mapper = inputMapper
                });
            }
            else throw new InvalidOperationException("Invalid lamda provided. Property is expected.");
            return this;
        }

        async Task<TResponse> DoSubmitAsync<TResponse> (
                Func<TModel, Task<TResponse>> call,
                Action<TResponse> onSuccess,
                Action<Dictionary<string, string[]>> onBadRequest,
                Action<TResponse> onError
            )
            where TResponse : ApiConsumedResponse
        {
            ClearErrors();
            if (TryGetModel(out TModel model))
            {
                var response = await call(model);
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
            else return null;
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

        class MappedContext
        {
            public string Name { get; set; }

            public Action<object> Set { get; set; }

            public Action<object> GetFrom { get; set; }

            public IComponentMapper Mapper { get; set; }
             
        }

    }
}
