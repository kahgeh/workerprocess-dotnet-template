using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WorkerProcess.Template.Infrastructure
{
    public class WorkerHealthCheck : IHealthCheck
    {
        private readonly IWorkerInfoProvider _workerInfoProvider;
        public WorkerHealthCheck(IWorkerInfoProvider workerInfoProvider)
        {
            _workerInfoProvider = workerInfoProvider;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
        {
            if (_workerInfoProvider.IsAlive())
            {
                return Task.FromResult(
                    HealthCheckResult.Healthy("Worker is running."));
            }

            return Task.FromResult(
                HealthCheckResult.Unhealthy("Worker is not running."));
        }
    }
}