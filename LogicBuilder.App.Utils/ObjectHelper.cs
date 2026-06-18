using LogicBuilder.App.Utils.Interfaces;

namespace LogicBuilder.App.Utils
{
    public class ObjectHelper : IObjectHelper
    {
        public object? Null => null;

        public bool IsNull(object? anyObject) => anyObject == null;
    }
}
