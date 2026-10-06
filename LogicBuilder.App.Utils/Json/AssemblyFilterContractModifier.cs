using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Serialization.Metadata;

namespace LogicBuilder.App.Utils.Json
{
    public class AssemblyFilterContractModifier(ObjectConverter objectConverter, params Assembly[] assemblies)
    {
        private readonly HashSet<Assembly> _allowedAssemblies = [.. assemblies];

        // This instanced method can be passed directly as a modifier delegate
        public void InjectObjectConverterByAssembly(JsonTypeInfo typeInfo)
        {
            if (typeInfo.Kind != JsonTypeInfoKind.Object) return;

            // Safely check if the type's assembly is in this specific instance's list
            if (_allowedAssemblies.Contains(typeInfo.Type.Assembly))
            {
                foreach (JsonPropertyInfo property in typeInfo.Properties)
                {
                    if (property.PropertyType == typeof(object))
                    {
                        property.CustomConverter = objectConverter;
                    }
                }
            }
        }
    }
}
