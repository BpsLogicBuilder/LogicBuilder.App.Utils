using Contoso.Test.Flow.Cache;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Contoso.Test.Flow
{
    public class CustomActions(ILogger<CustomActions> logger, FlowDataCache flowDataCache) : ICustomActions
    {
        private readonly ILogger<CustomActions> logger = logger;
        private readonly FlowDataCache flowDataCache = flowDataCache;

        public void WriteToLog(string message) => this.logger.LogInformation(message);

        public Task SetValueAync(string key, object value)
            => Task.Run(() => this.flowDataCache.Items[key] = value);
    }
}
