using LogicBuilder.App.Utils;
using LogicBuilder.App.Utils.Interfaces;
using LogicBuilder.App.Utils.Rules;
using LogicBuilder.App.Utils.Rules.Interfaces;
using LogicBuilder.App.Utils.Web;
using LogicBuilder.App.Utils.Web.Interfaces;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LogicBuilder.App.Utils.Tests, PublicKey=002400000480000094000000060200000024000052534131000400000100010059b59302e7303accd5cc84fd482cae54dea8d8b8de7faaef37abbac4b08e3d91283087f48ae04c4fdd117752a3fcafcda61cd2099e2d5432b9bce70e5fe083b15e43cd652617b06dc1422d347ffe7b2aeb7b466e567c6988f26dccbf9723b4b57b1aeaa0a2dbd00478d7135da9bb04a6138d5f29e54ac7e9ac9ae3b7956cf6c2")]
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
                .AddAppUtilsEnvironmentHelpers()
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

        public static IServiceCollection AddAppUtilsEnvironmentHelpers(this IServiceCollection services)
        {
            return services
                .AddTransient<IEnvironmentHelpers, EnvironmentHelpers>();
        }

        public static IServiceCollection AddAppUtilsGenericsHelpers(this IServiceCollection services)
        {
            return services
                .AddTransient<IGenericsHelpers, GenericsHelpers>();
        }

        public static IServiceCollection AddAppUtilsHttpClientHelper(this IServiceCollection services)
        {
            return services
                .AddTransient<IHttpClientHelper, HttpClientHelper>();
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
