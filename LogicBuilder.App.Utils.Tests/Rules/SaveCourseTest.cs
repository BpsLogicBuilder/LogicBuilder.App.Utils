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
    public class SaveCourseTest
    {
        public SaveCourseTest(ITestOutputHelper output)
        {
            this.output = output;
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        private readonly ITestOutputHelper output;
        #endregion Fields

        [Fact]
        public void SaveCourse()
        {
            //arrange
            IFlowManager flowManager = serviceProvider.GetRequiredService<IFlowManager>();
            flowManager.FlowDataCache.Request = new SaveEntityRequest
            {
                Entity = new CourseModel
                {
                    EntityState = LogicBuilder.Domain.EntityStateType.Modified,
                    CourseID = 1111,
                    Credits = 4,
                    DepartmentID = 2,
                    Title = "Chemistry"
                }
            };

            //act
            System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();
            flowManager.Start("savecourse");
            stopWatch.Stop();
            this.output.WriteLine("Saving valid course  = {0}", stopWatch.Elapsed.TotalMilliseconds);

            //assert
            Assert.True(flowManager.FlowDataCache.Response.Success);
        }

        [Fact]
        public void SaveInvalidCourse()
        {
            //arrange
            IFlowManager flowManager = serviceProvider.GetRequiredService<IFlowManager>();
            flowManager.FlowDataCache.Request = new SaveEntityRequest
            {
                Entity = new CourseModel
                {
                    EntityState = LogicBuilder.Domain.EntityStateType.Modified,
                    CourseID = 0,
                    Credits = 6,
                    DepartmentID = 0,
                    Title = ""
                }
            };

            //act
            System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();
            flowManager.Start("savecourse");
            stopWatch.Stop();
            this.output.WriteLine("Saving invalid course  = {0}", stopWatch.Elapsed.TotalMilliseconds);

            //assert
            Assert.False(flowManager.FlowDataCache.Response.Success);
            Assert.Equal(4, flowManager.FlowDataCache.Response.ErrorMessages.Count);
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
                            typeof(CourseModel).Assembly,
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
