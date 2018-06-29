using NetCore.Apis.Client.UI;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xamarin.Forms;

namespace MobileUI.Mapping
{
    static class ModelMapperExtensions
    {

        public static void Bind<TModel>(this ModelMapper<TModel> mapper, Expression<Func<TModel, string>> lamda
                                    , Entry entry, StackLayout errors)
            where TModel : class, new()
                => mapper.Bind(lamda, new EntryMapping(entry, errors));

        public static void Bind<TModel>(this ModelMapper<TModel> mapper, Expression<Func<TModel, int>> lamda
                            , Entry entry, StackLayout errors)
            where TModel : class, new()
            => mapper.Bind(lamda, new IntMapper(entry, errors));

    }
}
