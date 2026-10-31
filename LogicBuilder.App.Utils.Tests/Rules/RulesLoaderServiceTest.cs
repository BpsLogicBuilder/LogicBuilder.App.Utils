using Contoso.Domain.Entities;
using Contoso.Test.Business.Requests;
using LogicBuilder.App.Utils.Rules;
using LogicBuilder.RulesDirector;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace LogicBuilder.App.Utils.Tests.Rules
{
    public class RulesLoaderServiceTest
    {
        private sealed class NonPumpingSynchronizationContext : SynchronizationContext
        {
            public override void Post(SendOrPostCallback d, object? state) { /* never runs, like a blocked UI thread */ }
        }

        private static readonly List<Assembly> referenceAssemblies = [
            typeof(Interfaces.ITypeHelper).Assembly,
            typeof(IBaseRequest).Assembly,
            typeof(CourseModel).Assembly,
            typeof(DirectorBase).Assembly,
            typeof(string).Assembly
        ];
        readonly string embeddedResourcePath = "Contoso.Test.Flow.Rulesets";

        [Fact]
        public void LoadRulesAsync_BlockedOnContextThread_DoesNotDeadlock()
        {
            RulesCache? result = null;
            Exception? error = null;

            Thread thread = new(() =>
            {
                SynchronizationContext.SetSynchronizationContext(new NonPumpingSynchronizationContext());
                try
                {
                    result = RulesLoaderService.LoadRules
                    (
                        new RulesLoaderRequest
                        (
                            embeddedResourcePath,
                            typeof(Contoso.Test.Flow.FlowActivity),
                            referenceAssemblies
                        )
                    ).GetAwaiter().GetResult();
                }
                catch (Exception ex) { error = ex; }
            });
            thread.Start();

            Assert.True(thread.Join(TimeSpan.FromSeconds(5)), "Deadlock: continuation was posted to the blocked context.");
            Assert.Null(error);
            Assert.NotNull(result);
        }
    }
}
