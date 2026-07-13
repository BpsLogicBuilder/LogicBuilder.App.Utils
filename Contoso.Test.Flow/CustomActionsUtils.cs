using LogicBuilder.Attributes;
using System.Threading.Tasks;

namespace Contoso.Test.Flow
{
    public static class CustomActionsUtils
    {
        [AlsoKnownAs("WriteToLog")]
        public static void WriteToLog(ICustomActions customActions, string message) => customActions.WriteToLog(message);

        [AlsoKnownAs("SetValueAync")]
        public static Task SetValueAync(ICustomActions customActions, string key, object value) => customActions.SetValueAync(key, value);
    }
}
