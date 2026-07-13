using LogicBuilder.App.Utils;
using LogicBuilder.App.Utils.Interfaces;
using LogicBuilder.App.Utils.Rules;
using LogicBuilder.App.Utils.Rules.Interfaces;
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
                .AddAppUtilsDictionaryHelper()
                .AddAppUtilsGenericsHelpers()
                .AddAppUtilsHttpClientHelper()
                .AddAppUtilsMappingOperations()
                .AddAppUtilsObjectHelper()
                .AddAppUtilsRulesLoader()
                .AddAppUtilsStringHelper()
                .AddAppUtilsTypeHelper();
        }

        public static IServiceCollection AddAppUtilsDictionaryHelper(this IServiceCollection services)
        {
            return services
                .AddTransient<IDictionaryHelper, DictionaryHelper>();
        }

        public static IServiceCollection AddAppUtilsGenericsHelpers(this IServiceCollection services)
        {
            return services
                .AddTransient<IGenericsHelpers, GenericsHelpers>();
        }

        public static IServiceCollection AddAppUtilsHttpClientHelper(this IServiceCollection services)
        {
            return services
                .AddTransient<IHttpClientHelper, HttpClientHelper>()
                .AddHttpClient();
        }

        public static IServiceCollection AddAppUtilsMappingOperations(this IServiceCollection services)
        {
            return services
                .AddTransient<IMappingOperations, MappingOperations>();
        }

        public static IServiceCollection AddAppUtilsObjectHelper(this IServiceCollection services)
        {
            return services
                .AddTransient<IObjectHelper, ObjectHelper>();
        }

        public static IServiceCollection AddAppUtilsRulesLoader(this IServiceCollection services)
        {
            return services
                .AddTransient<IRulesLoader, RulesLoader>()
                .AddTransient<IRulesSerializer, RulesSerializer>();
        }

        public static IServiceCollection AddAppUtilsStringHelper(this IServiceCollection services)
        {
            return services
                .AddTransient<IStringHelper, StringHelper>();
        }

        public static IServiceCollection AddAppUtilsTypeHelper(this IServiceCollection services)
        {
            return services
                .AddTransient<ITypeHelper, TypeHelper>();
        }
    }
}
