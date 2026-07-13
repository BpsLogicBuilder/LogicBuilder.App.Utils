using Contoso.Domain;

namespace Contoso.Test.Business.Requests
{
    public class SaveEntityRequest : IBaseRequest
    {
        public EntityModelBase Entity { get; set; }
    }
}
