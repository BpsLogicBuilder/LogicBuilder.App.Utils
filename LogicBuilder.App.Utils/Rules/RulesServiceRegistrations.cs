using LogicBuilder.App.Utils.Rules;
using LogicBuilder.RulesDirector;

#pragma warning disable IDE0130 //Microsoft recommended namespace for service registrations
namespace Microsoft.Extensions.DependencyInjection
#pragma warning restore IDE0130
{
    public static class RulesServiceRegistrations
    {
        public static IServiceCollection AddRulesCacheService(this IServiceCollection services, RulesLoaderRequest rulesLoaderRequest)
        {
            IRulesCache rulesCache = RulesLoaderService.LoadRules(rulesLoaderRequest).GetAwaiter().GetResult();
            return services
                .AddSingleton(sp => rulesCache);
        }
    }
}
