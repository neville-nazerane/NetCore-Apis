using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

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


        class MappedContext
        {
            internal Action<object> Set { get; set; }
            
            internal Action<object> GetFrom { get; set; }

        }

    }
}
