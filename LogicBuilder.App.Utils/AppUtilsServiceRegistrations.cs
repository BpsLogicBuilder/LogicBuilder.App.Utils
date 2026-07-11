using LogicBuilder.App.Utils;
using LogicBuilder.App.Utils.Interfaces;
using LogicBuilder.App.Utils.Web;
using LogicBuilder.App.Utils.Web.Interfaces;

#pragma warning disable IDE0130 //Microsoft recommended namespace for service registrations
namespace Microsoft.Extensions.DependencyInjection
#pragma warning restore IDE0130
{
    public static class AppUtilsServiceRegistrations
    {
        public static IServiceCollection AddAppUtilsServices(this IServiceCollection services)
        {
            return services
                .AddTransient<IDictionaryHelper, DictionaryHelper>()
                .AddTransient<IGenericsHelpers, GenericsHelpers>()
                .AddTransient<IHttpClientHelper, HttpClientHelper>()
                .AddHttpClient()
                .AddTransient<IMappingOperations, MappingOperations>()
                .AddTransient<IObjectHelper, ObjectHelper>()
                .AddTransient<IStringHelper, StringHelper>()
                .AddTransient<ITypeHelper, TypeHelper>();
        }
    }
}
