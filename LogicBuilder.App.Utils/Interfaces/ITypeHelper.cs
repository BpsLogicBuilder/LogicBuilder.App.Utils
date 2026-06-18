using LogicBuilder.Attributes;
using System;

namespace LogicBuilder.App.Utils.Interfaces
{
    public interface ITypeHelper
    {
        object GetPropertyValue(object item, string propertyName);

        [AlsoKnownAs("Get Type")]
        Type GetType([ParameterEditorControl(ParameterControlType.TypeAutoComplete)] string assemblyQualifiedTypeName);
        
        string ToTypeString(Type type);

        bool TryParse(string toParse, Type type, out object? result);
    }
}
