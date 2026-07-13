using Contoso.Domain.Entities;
using Contoso.Test.Business.Requests;
using Contoso.Test.Flow;
using Contoso.Test.Flow.Cache;
using LogicBuilder.RulesDirector;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace LogicBuilder.App.Utils.Tests.Rules
{
    public class SaveStudentTest
    {
        public SaveStudentTest(ITestOutputHelper output)
        {
            this.output = output;
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        private readonly ITestOutputHelper output;
        #endregion Fields

        [Fact]
        public void SaveStudent()
        {
            //arrange
            IFlowManager flowManager = serviceProvider.GetRequiredService<IFlowManager>();
            flowManager.FlowDataCache.Request = new SaveEntityRequest
            {
                Entity = new StudentModel
                {
                    EntityState = LogicBuilder.Domain.EntityStateType.Modified,
                    FirstName = "John",
                    LastName = "Hopkins",
                    EnrollmentDate = new DateTime(2018, 3, 3, 0, 0, 0, DateTimeKind.Unspecified)
                }
            };

            //act
            System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();
            flowManager.Start("savestudent");
            stopWatch.Stop();
            this.output.WriteLine("Saving valid student  = {0}", stopWatch.Elapsed.TotalMilliseconds);

            //assert
            Assert.True(flowManager.FlowDataCache.Response.Success);
        }

        [Fact]
        public void SaveInvalidStudent()
        {
            //arrange
            IFlowManager flowManager = serviceProvider.GetRequiredService<IFlowManager>();
            flowManager.FlowDataCache.Request = new SaveEntityRequest
            {
                Entity = new StudentModel
                {
                    EntityState = LogicBuilder.Domain.EntityStateType.Modified,
                    FirstName = "",
                    LastName = "",
                    EnrollmentDate = default
                }
            };

            //act
            System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();
            flowManager.Start("savestudent");
            stopWatch.Stop();
            this.output.WriteLine("Saving invalid student  = {0}", stopWatch.Elapsed.TotalMilliseconds);

            //assert
            Assert.False(flowManager.FlowDataCache.Response.Success);
            Assert.Equal(3, flowManager.FlowDataCache.Response.ErrorMessages.Count);
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
