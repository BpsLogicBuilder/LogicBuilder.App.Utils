using LogicBuilder.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LogicBuilder.App.Utils
{
    public static class ServiceProviderUtils<TService> where TService : notnull
    {
        [AlsoKnownAs("Get Required Service")]
        public static TService GetRequiredService(IServiceProvider serviceProvider)
            => serviceProvider.GetRequiredService<TService>();
    }
}
