namespace LogicBuilder.App.Utils.Interfaces
{
    public interface IEnvironmentHelpers
    {
        string? GetEnvironmentVariable(string variableName, string? defaultValue = null);
    }
}
