using LogicBuilder.App.Utils.Interfaces;
using LogicBuilder.Attributes;
using System;

namespace LogicBuilder.App.Utils
{
    public static class TypeUtilities
    {
        public static object GetPropertyValue(ITypeHelper typeHelper, object item, string propertyName)
            => typeHelper.GetPropertyValue(item, propertyName);

        [AlsoKnownAs("Get Type")]
        public static Type GetType(ITypeHelper typeHelper, [ParameterEditorControl(ParameterControlType.TypeAutoComplete)] string assemblyQualifiedTypeName)
            => typeHelper.GetType(assemblyQualifiedTypeName);

        public static string ToTypeString(ITypeHelper typeHelper, Type type) 
            => typeHelper.ToTypeString(type);

        public static bool TryParse(ITypeHelper typeHelper, string toParse, Type type, out object? result)
        {
            bool success = typeHelper.TryParse(toParse, type, out object? outResult);
            result = outResult;
            return success;
        }
    }
}
