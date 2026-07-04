using LogicBuilder.App.Utils.Interfaces;

namespace LogicBuilder.App.Utils
{
    public static class ObjectUtilities
    {
        public static object? Null => null;

        public static bool IsNull(IObjectHelper objectHelper,  object? anyObject) => objectHelper.IsNull(anyObject);
    }
}
