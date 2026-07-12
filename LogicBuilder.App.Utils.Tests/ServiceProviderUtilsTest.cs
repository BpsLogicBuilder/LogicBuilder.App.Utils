using AutoMapper;
using LogicBuilder.App.Utils.Interfaces;
using LogicBuilder.App.Utils.Web.Interfaces;
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
            IDictionaryHelper dictionaryHelper = ServiceProviderUtils<IDictionaryHelper>.GetRequiredService(serviceProvider);
            IGenericsHelpers genericsHelpers = ServiceProviderUtils<IGenericsHelpers>.GetRequiredService(serviceProvider);
            IHttpClientHelper httpClientHelper = ServiceProviderUtils<IHttpClientHelper>.GetRequiredService(serviceProvider);
            IMappingOperations mappingOperations = ServiceProviderUtils<IMappingOperations>.GetRequiredService(serviceProvider);
            IObjectHelper objectHelper = ServiceProviderUtils<IObjectHelper>.GetRequiredService(serviceProvider);
            IStringHelper stringHelper = ServiceProviderUtils<IStringHelper>.GetRequiredService(serviceProvider);
            ITypeHelper typeHelper = ServiceProviderUtils<ITypeHelper>.GetRequiredService(serviceProvider);

            //assert
            Assert.NotNull(dictionaryHelper);
            Assert.NotNull(genericsHelpers);
            Assert.NotNull(httpClientHelper);
            Assert.NotNull(mappingOperations);
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
                .AddSingleton<IConfigurationProvider>
                (
                    new MapperConfiguration(cfg =>
                    {
                    }, Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory.Instance)
                )
                .AddTransient<IMapper>(sp => new Mapper(sp.GetRequiredService<IConfigurationProvider>(), sp.GetService))//mapper configuration reqguired for MappingOperations service
                .AddLogging()
                .BuildServiceProvider();
        }
        #endregion Helpers
    }
}
