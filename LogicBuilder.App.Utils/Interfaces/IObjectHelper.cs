using LogicBuilder.Attributes;

namespace LogicBuilder.App.Utils.Interfaces
{
    public interface IObjectHelper
    {
        object? Null { get; }

        [AlsoKnownAs("ObjectIsNull")]
        public bool IsNull(object? anyObject);
    }
}
