using LogicBuilder.Workflow.Activities.Rules;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LogicBuilder.App.Utils.Rules
{
    internal static class RulesLoaderService
    {
        public static async Task<RulesCache> LoadRules(RulesLoaderRequest rulesLoaderRequest)
        {
            return await LoadRules(new RulesLoader(new RulesSerializer()), rulesLoaderRequest);
        }

        static async Task<RulesCache> LoadRules(RulesLoader rulesLoader, RulesLoaderRequest rulesLoaderRequest)
        {
            RulesCache cache = new(new ConcurrentDictionary<string, RuleEngine>(), new ConcurrentDictionary<string, string>());

            var assembly = rulesLoaderRequest.FlowActivityType.Assembly;
            string[] embeddedResources = GetResourceNames(assembly, rulesLoaderRequest.EmbeddedResourcesPath);

            Dictionary<string, string> rules = embeddedResources
                                                .Where(f => f.EndsWith(".module"))
                                                .ToDictionary(f => GetKey(f).ToLowerInvariant());

            Dictionary<string, string> resources = embeddedResources
                                                .Where(f => f.EndsWith(".resources"))
                                                .ToDictionary(f => GetKey(f).ToLowerInvariant());

            await Task.WhenAll
            (
                rules.Keys.Select
                (
                    key => rulesLoader.LoadRulesOnStartUp
                    (
                        new RulesModule
                        (
                            key,
                            GetBytes(resources[key], assembly),
                            GetBytes(rules[key], assembly)
                        ),
                        cache,
                        rulesLoaderRequest
                    )
                )
            );

            return cache;

            static string GetKey(string fullResourceName)
                => Path.GetExtension(Path.GetFileNameWithoutExtension(fullResourceName)).Substring(1);
            //Gets the full name first: Contoso.Bsl.Flow.Rulesets.savecourse.module
            //Then GetFileNameWithoutExtension returns: Contoso.Bsl.Flow.Rulesets.savecourse
            //Finally Path.GetExtension and the range operator return: savecourse
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
            using Stream platformStream = assembly.GetManifestResourceStream(file);
            byte[] byteArray = new byte[platformStream.Length];
            using var memoryStream = new MemoryStream();
            int read;
            while ((read = platformStream.Read(byteArray, 0, byteArray.Length)) > 0)
            {
                memoryStream.Write(byteArray, 0, read);
            }
            return byteArray;
        }
    }
}
