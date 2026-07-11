using LogicBuilder.App.Utils.Interfaces;
using LogicBuilder.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicBuilder.App.Utils
{
    public static class StringUtils
    {
        public static bool IsValidEmail(IStringHelper stringHelper, string email) 
            => stringHelper.IsValidEmail(email);

        [AlsoKnownAs("StringIsNullOrEmpty")]
        public static bool StringIsNullOrEmpty(IStringHelper stringHelper, string value) => stringHelper.StringIsNullOrEmpty(value);
    }
}
