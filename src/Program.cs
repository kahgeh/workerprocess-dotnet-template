using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace WorkerProcess.Template
{
    public class Program
    {
        private static int PastAndCurrentJobQueueSize = 10;
        private static WorkerInfo WorkerInfo = new WorkerInfo();
        public static void Main(string[] args)
        {
            Task.Run(() => CreateHostBuilder(args).Build().Run());
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IWorkerInfoProvider>(new WorkerInfoProvider(WorkerInfo));
                })
                .UseStartup<HttpStartup>();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IWorkerInfoStore>(new WorkerInfoStore(WorkerInfo, PastAndCurrentJobQueueSize));
                    services.AddHostedService<Worker>();
                });
    }
}
