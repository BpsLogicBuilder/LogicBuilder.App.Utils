using LogicBuilder.App.Utils.Interfaces;
using Microsoft.Extensions.Configuration;

namespace LogicBuilder.App.Utils
{
    public class EnvironmentHelpers(IConfiguration configuration) : IEnvironmentHelpers
    {
        private readonly IConfiguration configuration = configuration;

        public string? GetEnvironmentVariable(string variableName, string? defaultValue = null)
        {
            string? returnValue = configuration[variableName];
            return !string.IsNullOrEmpty(returnValue) ? returnValue : defaultValue;
        }
    }
}
