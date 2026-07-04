using LogicBuilder.App.Utils.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace LogicBuilder.App.Utils.Tests
{
    public class ServicesHelperTest
    {
        static ServicesHelperTest()
        {
            RegisterServiceProvider();
        }

        private static IServiceProvider serviceProvider;

        [Fact]
        public void GetRequiredServiceCanInitializeAppUtilsServices()
        {
            //act
            IGenericsHelpers genericsHelpers = ServicesHelper<IGenericsHelpers>.GetRequiredService(serviceProvider);
            IObjectHelper objectHelper = ServicesHelper<IObjectHelper>.GetRequiredService(serviceProvider);
            IStringHelper stringHelper = ServicesHelper<IStringHelper>.GetRequiredService(serviceProvider);
            ITypeHelper typeHelper = ServicesHelper<ITypeHelper>.GetRequiredService(serviceProvider);

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
