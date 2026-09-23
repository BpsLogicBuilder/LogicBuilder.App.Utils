using LogicBuilder.App.Utils.Interfaces;
using LogicBuilder.Attributes;

namespace LogicBuilder.App.Utils
{
    public static class EnvironmentUtils
    {
        [AlsoKnownAs("GetEnvironmentVariable")]
        public static string? GetEnvironmentVariable(IEnvironmentHelpers environmentHelpers, string variableName, string? defaultValue = null) 
            => environmentHelpers.GetEnvironmentVariable(variableName, defaultValue);
    }
}
