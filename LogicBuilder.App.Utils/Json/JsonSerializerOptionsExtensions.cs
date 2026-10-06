using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace LogicBuilder.App.Utils.Json
{
    public static class JsonSerializerOptionsExtensions
    {
        public static JsonSerializerOptions AddAssemblyFilteredConverter(this JsonSerializerOptions options, ObjectConverter objectConverter, params Assembly[] assemblies)
        {
            // The state lives safely inside this isolated instance, not globally
            var modifier = new AssemblyFilterContractModifier(objectConverter, assemblies);

            options.TypeInfoResolver ??= new DefaultJsonTypeInfoResolver();

            if (options.TypeInfoResolver is DefaultJsonTypeInfoResolver resolver)
            {
                resolver.Modifiers.Add(modifier.InjectObjectConverterByAssembly);
            }

            return options;
        }
    }
}
