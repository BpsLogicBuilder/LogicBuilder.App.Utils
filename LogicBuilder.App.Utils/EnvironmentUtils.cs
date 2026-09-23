using LogicBuilder.App.Utils.Interfaces;

namespace LogicBuilder.App.Utils
{
    public static class EnvironmentUtils
    {
        public static string? GetEnvironmentVariable(IEnvironmentHelpers environmentHelpers, string variableName, string? defaultValue = null) 
            => environmentHelpers.GetEnvironmentVariable(variableName, defaultValue);
    }
}
