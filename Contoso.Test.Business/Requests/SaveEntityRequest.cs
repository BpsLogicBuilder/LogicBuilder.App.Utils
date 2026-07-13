using LogicBuilder.Domain;

namespace Contoso.Test.Business.Requests
{
    public class SaveEntityRequest : IBaseRequest
    {
        public BaseModel Entity { get; set; }
    }
}
