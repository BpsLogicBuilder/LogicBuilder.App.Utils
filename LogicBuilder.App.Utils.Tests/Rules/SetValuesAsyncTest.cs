using Contoso.Test.Business.Requests;
using Contoso.Test.Flow;
using Contoso.Test.Flow.Cache;
using LogicBuilder.RulesDirector;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace LogicBuilder.App.Utils.Tests.Rules
{
    public class SetValuesAsyncTest
    {
        public SetValuesAsyncTest(ITestOutputHelper output)
        {
            this.output = output;
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        private readonly ITestOutputHelper output;
        #endregion Fields

        [Fact]
        public void SetValues()
        {
            //arrange
            IFlowManager flowManager = serviceProvider.GetRequiredService<IFlowManager>();

            //act
            System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();
            flowManager.Start("setvaluesasync");
            stopWatch.Stop();
            this.output.WriteLine("Setting values async  = {0}", stopWatch.Elapsed.TotalMilliseconds);

            //assert
            Assert.Equal("A", flowManager.FlowDataCache.Items["A"].ToString());
            Assert.Equal("B", flowManager.FlowDataCache.Items["B"].ToString());
            Assert.Equal("C", flowManager.FlowDataCache.Items["C"].ToString());
            Assert.True(flowManager.FlowDataCache.Response.Success);
        }

        #region Helpers
        [MemberNotNull(nameof(serviceProvider))]
        private void Initialize()
        {
            serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddTransient<IFlowManager, FlowManager>()
                .AddTransient<DirectorFactory, DirectorFactory>()
                .AddTransient<ICustomActions, CustomActions>()
                .AddScoped<FlowDataCache>()
                .AddScoped<Progress>()
                .AddAppUtilsGenericsHelpers()
                .AddAppUtilsObjectHelper()
                .AddRulesCacheService
                (
                    new Utils.Rules.RulesLoaderRequest
                    (
                        "Contoso.Test.Flow.Rulesets",
                        typeof(FlowActivity),
                        [
                            typeof(Interfaces.ITypeHelper).Assembly,
                            typeof(IBaseRequest).Assembly,
                            typeof(Contoso.Domain.EntityModelBase).Assembly,
                            typeof(DirectorBase).Assembly,
                            typeof(string).Assembly
                        ]
                    )
                )
                .BuildServiceProvider();
        }
        #endregion Helpers
    }
}
