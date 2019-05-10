using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WorkerProcess.Template.Contracts;

namespace WorkerProcess.Template
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IWorkerInfoStore _workerInfoStore;
        public Worker(ILogger<Worker> logger, IWorkerInfoStore workerInfoStore)
        {
            _logger = logger;
            _workerInfoStore = workerInfoStore;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _workerInfoStore.AddJob(new Job
                    {
                        Name = $"NOOP-{DateTime.UtcNow.Ticks}",
                        StartUtc = DateTime.UtcNow,
                        Status = JobStatus.InProgress,
                        Type = "NOOP"
                    });
                    _logger.LogInformation($"Worker running at: {DateTime.Now}");
                    await Task.Delay(1000, stoppingToken);
                    _workerInfoStore.AssignStatus(JobStatus.Successful);
                }
            }
            finally
            {
                _workerInfoStore.MarkAsNotAlive();
            }
        }
    }
}
