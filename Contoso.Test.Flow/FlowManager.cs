using Contoso.Test.Business.Responses;
using Contoso.Test.Flow.Cache;
using LogicBuilder.RulesDirector;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading;

namespace Contoso.Test.Flow
{
    public class FlowManager : IFlowManager
    {
        public FlowManager(DirectorFactory directorFactory,
            ILogger<FlowManager> logger, 
            Progress progress,
            FlowDataCache flowDataCache,
            IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.Progress = progress;
            this.FlowDataCache = flowDataCache;
            this.ServiceProvider = serviceProvider;
            this.Director = directorFactory.Create(this);
            this.FlowActivity = FlowActivityFactory.Create(this);
        }

        public IFlowActivity FlowActivity { get; }
        public FlowDataCache FlowDataCache { get; }
        public Progress Progress { get; }

        private readonly ILogger<FlowManager> logger;

        public DirectorBase Director { get; }

        public IServiceProvider ServiceProvider { get; }

        public void FlowComplete()
        {
            if (FlowDataCache.Response == null)
            {
                logger.LogError("Response cannot be null.");
                throw new InvalidOperationException("Response cannot be null.");
            }
        }

        public void SetCurrentBusinessBackupData() {}

        public void Terminate() => throw new NotImplementedException();

        public void Start(string module)
        {
            try
            {
                System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();
                this.Director.StartInitialFlow(module);
                stopWatch.Stop();
                logger.LogInformation("this.Director.StartInitialFlow: {0}", stopWatch.Elapsed.TotalMilliseconds);
            }
            catch (Exception ex) when (!IsCriticalException(ex))
            {
                FlowDataCache.Response = new ErrorResponse
                {
                    Success = false,
                    ErrorMessages = [ex.Message]
                };
                logger.LogWarning(0, "Progress Start {Ptogress}", JsonSerializer.Serialize(this.Progress));
                this.logger.LogError(ex, ex.Message);
            }

            static bool IsCriticalException(Exception ex)
            {
                return ex is OutOfMemoryException
                    or ThreadAbortException
                    or StackOverflowException
                    or ThreadInterruptedException;
            }
        }
    }
}
