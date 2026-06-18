using LogicBuilder.Attributes;

namespace LogicBuilder.App.Utils.Interfaces
{
    public interface IStringHelper
    {
        bool IsValidEmail(string email);

        [AlsoKnownAs("StringIsNullOrEmpty")]
        bool StringIsNullOrEmpty(string value);
    }
}
