using Contoso.Domain.Entities;
using Contoso.Test.Business.Requests;
using Contoso.Test.Flow;
using Contoso.Test.Flow.Cache;
using LogicBuilder.App.Utils.Rules;
using LogicBuilder.App.Utils.Rules.Interfaces;
using LogicBuilder.RulesDirector;
using LogicBuilder.Workflow.Activities.Rules;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LogicBuilder.App.Utils.Tests.Rules
{
    public class RulesLoaderTest
    {
        public RulesLoaderTest(ITestOutputHelper output)
        {
            this.output = output;
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        private readonly ITestOutputHelper output;
        private static readonly List<Assembly> referenceAssemblies = [
                            typeof(Interfaces.ITypeHelper).Assembly,
                            typeof(IBaseRequest).Assembly,
                            typeof(CourseModel).Assembly,
                            typeof(DirectorBase).Assembly,
                            typeof(string).Assembly
                        ];

        #endregion Fields

        [Fact]
        public async Task CanLoadAndExecuteRules()
        {
            //arrange
            IRulesLoader rulesLoader = serviceProvider.GetRequiredService<IRulesLoader>();
            IRulesCache rulesCache = serviceProvider.GetRequiredService<IRulesCache>();
            IFlowManager flowManager = serviceProvider.GetRequiredService<IFlowManager>();
            string mainModuleName = "savecourse";
            string childModuleName = "validatecourse";
            string embeddedResourcePath = "Contoso.Test.Flow.Rulesets";
            flowManager.FlowDataCache.Request = new SaveEntityRequest
            {
                Entity = new CourseModel
                {
                    EntityState = Domain.EntityStateType.Modified,
                    CourseID = 1111,
                    Credits = 4,
                    DepartmentID = 2,
                    Title = "Chemistry"
                }
            };

            //act
            await rulesLoader.LoadRules
            (
                GetRulesModule(mainModuleName, embeddedResourcePath),
                rulesCache,
                new RulesLoaderRequest(embeddedResourcePath, typeof(FlowActivity), referenceAssemblies)
            );
            await rulesLoader.LoadRules
            (
                GetRulesModule(childModuleName, embeddedResourcePath),
                rulesCache,
                new RulesLoaderRequest(embeddedResourcePath, typeof(FlowActivity), referenceAssemblies)
            );
            System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();
            flowManager.Start(mainModuleName);
            stopWatch.Stop();
            this.output.WriteLine("Saving valid course  = {0}", stopWatch.Elapsed.TotalMilliseconds);

            //assert
            Assert.True(flowManager.FlowDataCache.Response.Success);
        }

        [Fact]
        public async Task CanLoadAndReloadModuleThenExecuteRules()
        {
            //arrange
            IRulesLoader rulesLoader = serviceProvider.GetRequiredService<IRulesLoader>();
            IRulesCache rulesCache = serviceProvider.GetRequiredService<IRulesCache>();
            IFlowManager flowManager = serviceProvider.GetRequiredService<IFlowManager>();
            string mainModuleName = "savecourse";
            string childModuleName = "validatecourse";
            string embeddedResourcePath = "Contoso.Test.Flow.Rulesets";
            flowManager.FlowDataCache.Request = new SaveEntityRequest
            {
                Entity = new CourseModel
                {
                    EntityState = Domain.EntityStateType.Modified,
                    CourseID = 1111,
                    Credits = 4,
                    DepartmentID = 2,
                    Title = "Chemistry"
                }
            };

            //act
            await rulesLoader.LoadRules
            (
                GetRulesModule(mainModuleName, embeddedResourcePath),
                rulesCache,
                new RulesLoaderRequest(embeddedResourcePath, typeof(FlowActivity), referenceAssemblies)
            );
            await rulesLoader.LoadRules
            (
                GetRulesModule(childModuleName, embeddedResourcePath),
                rulesCache,
                new RulesLoaderRequest(embeddedResourcePath, typeof(FlowActivity), referenceAssemblies)
            );
            await rulesLoader.LoadRules
            (
                GetRulesModule(mainModuleName, embeddedResourcePath),
                rulesCache,
                new RulesLoaderRequest(embeddedResourcePath, typeof(FlowActivity), referenceAssemblies)
            );
            await rulesLoader.LoadRules
            (
                GetRulesModule(childModuleName, embeddedResourcePath),
                rulesCache,
                new RulesLoaderRequest(embeddedResourcePath, typeof(FlowActivity), referenceAssemblies)
            );
            System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();
            flowManager.Start(mainModuleName);
            stopWatch.Stop();
            this.output.WriteLine("Saving valid course  = {0}", stopWatch.Elapsed.TotalMilliseconds);

            //assert
            Assert.True(flowManager.FlowDataCache.Response.Success);
        }

        [Fact]
        public async Task FlowSuccessIsFalseWhenRulesModuleNotFound()
        {
            //arrange
            IFlowManager flowManager = serviceProvider.GetRequiredService<IFlowManager>();
            string mainModuleName = "savecourse";
            flowManager.FlowDataCache.Request = new SaveEntityRequest
            {
                Entity = new CourseModel
                {
                    EntityState = Domain.EntityStateType.Modified,
                    CourseID = 1111,
                    Credits = 4,
                    DepartmentID = 2,
                    Title = "Chemistry"
                }
            };

            //act
            System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();
            flowManager.Start(mainModuleName);
            stopWatch.Stop();
            this.output.WriteLine("Saving valid course  = {0}", stopWatch.Elapsed.TotalMilliseconds);

            //assert
            Assert.False(flowManager.FlowDataCache.Response.Success);
            Assert.Contains($"RuleEngine not found for {mainModuleName}.", flowManager.FlowDataCache.Response.ErrorMessages);
        }

        [Fact]
        public async Task LoadulesThrowsArgumentExceptionForEmptyRuleset()
        {
            //arrange
            string embeddedResourcePath = "Contoso.Test.Flow.InvalidRulesets";
            IRulesLoader rulesLoader = serviceProvider.GetRequiredService<IRulesLoader>();
            RulesModule emptyModule = GetRulesModule("empty", embeddedResourcePath);
            IRulesCache rulesCache = serviceProvider.GetRequiredService<IRulesCache>();

            //act and assert
            var exception = await Assert.ThrowsAsync<ArgumentException>
            (
                async () => await rulesLoader.LoadRules(emptyModule, rulesCache, new RulesLoaderRequest(embeddedResourcePath, typeof(FlowActivity), referenceAssemblies))
            );
            Assert.Contains($"Ruleset for module empty is invalid.", exception.Message);
        }

        [Fact]
        public async Task LoadulesOnStartupThrowsArgumentExceptionForEmptyRuleset()
        {
            //arrange
            string embeddedResourcePath = "Contoso.Test.Flow.InvalidRulesets";
            IRulesLoader rulesLoader = serviceProvider.GetRequiredService<IRulesLoader>();
            RulesModule emptyModule = GetRulesModule("empty", embeddedResourcePath);
            IRulesCache rulesCache = serviceProvider.GetRequiredService<IRulesCache>();

            //act and assert
            var exception = await Assert.ThrowsAsync<ArgumentException>
            (
                async () => await rulesLoader.LoadRulesOnStartUp(emptyModule, rulesCache, new RulesLoaderRequest(embeddedResourcePath, typeof(FlowActivity), referenceAssemblies))
            );
            Assert.Contains($"Ruleset for module empty is invalid.", exception.Message);
        }

        #region Helpers
        [MemberNotNull(nameof(serviceProvider))]
        private void Initialize()
        {
            IRulesCache cache = new RulesCache(new ConcurrentDictionary<string, RuleEngine>(), new ConcurrentDictionary<string, string>());
            serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddTransient<IFlowManager, FlowManager>()
                .AddTransient<DirectorFactory, DirectorFactory>()
                .AddTransient<ICustomActions, CustomActions>()
                .AddScoped<FlowDataCache>()
                .AddScoped<Progress>()
                .AddAppUtilsGenericsHelpers()
                .AddAppUtilsObjectHelper()
                .AddAppUtilsRulesLoader()
                .AddSingleton(sp => cache)
                .BuildServiceProvider();
        }

        private static RulesModule GetRulesModule(string moduleName, string embeddedResourcePath)
        {
            Assembly assembly = typeof(FlowActivity).Assembly;
            string[] embeddedResources = GetResourceNames(assembly, $"{embeddedResourcePath}.{moduleName}");

            Dictionary<string, string> rules = embeddedResources
                                                .Where(f => f.EndsWith(".module"))
                                                .ToDictionary(f => GetKey(f).ToLowerInvariant());

            Dictionary<string, string> resources = embeddedResources
                                                .Where(f => f.EndsWith(".resources"))
                                                .ToDictionary(f => GetKey(f).ToLowerInvariant());

            string key = moduleName;
            return new RulesModule
            (
                key,
                GetBytes(resources[key], assembly),
                GetBytes(rules[key], assembly)
            );

            static string GetKey(string fullResourceName)
                => Path.GetExtension(Path.GetFileNameWithoutExtension(fullResourceName))[1..];
        }

        private static string[] GetResourceNames(Assembly assembly, string embeddedResourcesPath)
            =>
            [
                .. assembly.GetManifestResourceNames().Where
                (
                    res => res.StartsWith
                    (
                        $"{embeddedResourcesPath}.",
                        System.StringComparison.InvariantCultureIgnoreCase
                    )
                )
            ];

        private static byte[] GetBytes(string file, Assembly assembly)
        {
            using Stream platformStream = assembly.GetManifestResourceStream(file) ?? throw new InvalidOperationException("Rules resources not specified in the assembly.");
            byte[] byteArray = new byte[platformStream.Length];
            using var memoryStream = new MemoryStream();
            int read;
            while ((read = platformStream.Read(byteArray, 0, byteArray.Length)) > 0)
            {
                memoryStream.Write(byteArray, 0, read);
            }
            return byteArray;
        }
        #endregion Helpers
    }
}
