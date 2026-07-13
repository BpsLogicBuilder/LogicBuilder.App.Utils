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
    public class RulesSerializerTest
    {
        public RulesSerializerTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        private static readonly List<Assembly> referenceAssemblies = [
                            typeof(Interfaces.ITypeHelper).Assembly,
                            typeof(IBaseRequest).Assembly,
                            typeof(CourseModel).Assembly,
                            typeof(DirectorBase).Assembly,
                            typeof(string).Assembly
                        ];

        #endregion Fields

        [Fact]
        public async Task CanLoadInvalidRuleset()
        {
            //arrange
            IRulesSerializer rulesSerializer = serviceProvider.GetRequiredService<IRulesSerializer>();
            string embeddedResourcePath = "Contoso.Test.Flow.InvalidRulesets";
            RulesModule invalidModule = GetRulesModule("invalid", embeddedResourcePath);

            //act
            RuleSet? invalidRuleset = rulesSerializer.DeserializeRuleSetFile(invalidModule);

            //assert
            Assert.NotNull(invalidRuleset);
        }

        [Fact]
        public async Task RuleValidationForInvalidRuleseThrowArgumentException()
        {
            //arrange
            string embeddedResourcePath = "Contoso.Test.Flow.InvalidRulesets";
            IRulesSerializer rulesSerializer = serviceProvider.GetRequiredService<IRulesSerializer>();
            RulesModule invalidModule = GetRulesModule("invalid", embeddedResourcePath);
            RuleSet? invalidRuleset = rulesSerializer.DeserializeRuleSetFile(invalidModule);

            //act and assert
            ArgumentException exception = Assert.Throws<ArgumentException>
            (
                () => rulesSerializer.GetValidation(invalidRuleset!, new RulesLoaderRequest(embeddedResourcePath, typeof(FlowActivity), referenceAssemblies))
            );
            Assert.Contains($"Ruleset for module invalid is invalid.", exception.Message);
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
