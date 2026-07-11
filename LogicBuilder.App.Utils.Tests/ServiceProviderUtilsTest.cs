using LogicBuilder.App.Utils.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace LogicBuilder.App.Utils.Tests
{
    public class ServiceProviderUtilsTest
    {
        static ServiceProviderUtilsTest()
        {
            RegisterServiceProvider();
        }

        private static IServiceProvider serviceProvider;

        [Fact]
        public void GetRequiredServiceCanInitializeAppUtilsServices()
        {
            //act
            IGenericsHelpers genericsHelpers = ServiceProviderUtils<IGenericsHelpers>.GetRequiredService(serviceProvider);
            IObjectHelper objectHelper = ServiceProviderUtils<IObjectHelper>.GetRequiredService(serviceProvider);
            IStringHelper stringHelper = ServiceProviderUtils<IStringHelper>.GetRequiredService(serviceProvider);
            ITypeHelper typeHelper = ServiceProviderUtils<ITypeHelper>.GetRequiredService(serviceProvider);

            //assert
            Assert.NotNull(genericsHelpers);
            Assert.NotNull(objectHelper);
            Assert.NotNull(stringHelper);
            Assert.NotNull(typeHelper);
        }

        #region Helpers
        [MemberNotNull(nameof(serviceProvider))]
        private static void RegisterServiceProvider()
        {
            serviceProvider ??= new ServiceCollection()
                .AddAppUtilsServices()
                .AddLogging()
                .BuildServiceProvider();
        }
        #endregion Helpers
    }
}
