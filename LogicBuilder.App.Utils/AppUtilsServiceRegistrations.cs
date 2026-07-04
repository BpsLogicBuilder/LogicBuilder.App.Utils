using LogicBuilder.App.Utils;
using LogicBuilder.App.Utils.Interfaces;

#pragma warning disable IDE0130 //Microsoft recommended namespace for service registrations
namespace Microsoft.Extensions.DependencyInjection
#pragma warning restore IDE0130
{
    public static class AppUtilsServiceRegistrations
    {
        public static IServiceCollection AddAppUtilsServices(this IServiceCollection services)
        {
            return services
                .AddTransient<IGenericsHelpers, GenericsHelpers>()
                .AddTransient<IObjectHelper, ObjectHelper>()
                .AddTransient<IStringHelper, StringHelper>()
                .AddTransient<ITypeHelper, TypeHelper>();
        }
    }
}
