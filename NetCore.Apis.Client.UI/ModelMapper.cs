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
        private readonly List<MappedData> mappedCollection;

        public ModelMapper()
        {
            //mappers = new List<IInputMapper>();
            properties = typeof(TModel).GetProperties().ToDictionary(p => p.Name);
            mappedCollection = new List<MappedData>();
        }

        TModel _Model;
        public TModel Model
        {
            get => _Model ?? (_Model = GenerateModel());
            
        }

        public void Bind<T>(IInputMapper inputMapper, Expression<Func<TModel, T>> lamda)
        {

            mappedCollection.Add(new MappedData {
                
            });
        }

        TModel GenerateModel()
        {
            var model = new TModel();

            return model;
        }

        class MappedData
        {
            internal PropertyInfo Info;

            internal IInputMapper Mapper;

            internal void Set(object obj) => Info.SetValue(obj, Mapper.MappedData);

            internal void GetFrom(object obj) => Mapper.MappedData = Info.GetValue(obj);

        }

    }
}
