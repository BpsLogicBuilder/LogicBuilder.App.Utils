using LogicBuilder.App.Utils.Interfaces;

namespace LogicBuilder.App.Utils
{
    public class StringHelper : IStringHelper
    {
        public bool IsValidEmail(string email)
        {
            try
            {
                return new System.Net.Mail.MailAddress(email).Address == email;
            }
            catch (System.FormatException)
            {
                return false;
            }
            catch (System.ArgumentException)
            {
                return false;
            }
        }

        public bool StringIsNullOrEmpty(string value) => string.IsNullOrEmpty(value);
    }
}
