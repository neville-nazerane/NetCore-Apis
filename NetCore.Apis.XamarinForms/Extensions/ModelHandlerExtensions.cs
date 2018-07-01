using NetCore.Apis.XamarinForms.Mapping;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xamarin.Forms;

namespace NetCore.Apis.Client.UI
{
    public static class ModelHandlerExtensions
    {

        public static ModelHandler<TModel> Bind<TModel>(this ModelHandler<TModel> modelHandler,
                        Expression<Func<TModel, string>> lamda, Entry entry, StackLayout stackLayout = null)
            where TModel : class, new()
            => modelHandler.Bind(lamda, new EntryStringMapper(entry, stackLayout));

        public static ModelHandler<TModel> Bind<TModel>(this ModelHandler<TModel> modelHandler,
                Expression<Func<TModel, string>> lamda, Editor editor, StackLayout stackLayout = null)
            where TModel : class, new()
            => modelHandler.Bind(lamda, new EditorStringMapper(editor, stackLayout));

        public static ModelHandler<TModel> Bind<TModel>(this ModelHandler<TModel> modelHandler,
                Expression<Func<TModel, int>> lamda, Entry entry, StackLayout stackLayout = null)
            where TModel : class, new()
            => modelHandler.Bind(lamda, new EntryIntMapper(entry, stackLayout));

        public static ModelHandler<TModel> Bind<TModel>(this ModelHandler<TModel> modelHandler,
                Expression<Func<TModel, int?>> lamda, Entry entry, StackLayout stackLayout = null)
            where TModel : class, new()
            => modelHandler.Bind(lamda, new EntryIntNullMapper(entry, stackLayout));

        public static ModelHandler<TModel> Bind<TModel>(this ModelHandler<TModel> modelHandler,
                Expression<Func<TModel, double>> lamda, Entry entry, StackLayout stackLayout = null)
            where TModel : class, new()
            => modelHandler.Bind(lamda, new EntryDoubleMapper(entry, stackLayout));

        public static ModelHandler<TModel> Bind<TModel>(this ModelHandler<TModel> modelHandler,
                Expression<Func<TModel, double?>> lamda, Entry entry, StackLayout stackLayout = null)
            where TModel : class, new()
            => modelHandler.Bind(lamda, new EntryDoubleNullMapper(entry, stackLayout));

        public static ModelHandler<TModel> Bind<TModel>(this ModelHandler<TModel> modelHandler,
                Expression<Func<TModel, float>> lamda, Entry entry, StackLayout stackLayout = null)
            where TModel : class, new()
            => modelHandler.Bind(lamda, new EntryFloatMapper(entry, stackLayout));

        public static ModelHandler<TModel> Bind<TModel>(this ModelHandler<TModel> modelHandler,
                Expression<Func<TModel, float?>> lamda, Entry entry, StackLayout stackLayout = null)
            where TModel : class, new()
            => modelHandler.Bind(lamda, new EntryFloatNullMapper(entry, stackLayout));

        public static ModelHandler<TModel> Bind<TModel>(this ModelHandler<TModel> mapper,
            Expression<Func<TModel, DateTime>> lamda, DatePicker date, TimePicker time, StackLayout errors = null)
            where TModel : class, new()
            => mapper.Bind(lamda, new DateTimeMapper(date, time, errors));

        public static ModelHandler<TModel> Bind<TModel>(this ModelHandler<TModel> mapper,
            Expression<Func<TModel, DateTime>> lamda, DatePicker date, StackLayout errors = null)
            where TModel : class, new()
            => mapper.Bind(lamda, new DateTimeMapper(date, null, errors));

        public static ModelHandler<TModel> Bind<TModel>(this ModelHandler<TModel> mapper, 
            Expression<Func<TModel, DateTime>> lamda, TimePicker time, StackLayout errors = null)
            where TModel : class, new()
            => mapper.Bind(lamda, new DateTimeMapper(null, time, errors));

        public static ModelHandler<TModel> Bind<TModel>(this ModelHandler<TModel> mapper,
            Expression<Func<TModel, bool>> lamda, Switch component, StackLayout errors = null)
            where TModel : class, new()
                 => mapper.Bind(lamda, new SwitchBoolMapper(component, errors));


    }
}
