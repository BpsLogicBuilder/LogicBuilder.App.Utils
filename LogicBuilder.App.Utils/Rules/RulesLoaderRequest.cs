using System;
using System.Collections.Generic;
using System.Reflection;

namespace LogicBuilder.App.Utils.Rules
{
    /// <summary>
    /// Information needed to load and validate the rules.  Additional assemblies may be needed to support Logic Builder design time behavior.
    /// </summary>
    /// <param name="embeddedResourcesPath">Path to the ad=ssembly's embedded resources.  The default is <RootNamespace>.<Folder> e.g. Contoso.Bsl.Flow.Rulesets</param>
    /// <param name="flowActivityType">The flow activity type e.g. typeof(FlowActivity).</param>
    /// <param name="referenceAssemblies">List fo assemblies needed for validation and for the Logic Builder application to list types at design time.</param>
    public class RulesLoaderRequest(string embeddedResourcesPath, Type flowActivityType, List<Assembly> referenceAssemblies)
    {
        public string EmbeddedResourcesPath { get; } = embeddedResourcesPath;
        public Type FlowActivityType { get; } = flowActivityType;
        public List<Assembly> ReferenceAssemblies { get; } = referenceAssemblies;
    }
}
