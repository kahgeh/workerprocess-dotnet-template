
using System;

namespace WorkerProcess.Template.Contracts
{
    public class Job
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public TimeSpan ElapsedTime { get; set; }
        public JobStatus Status { get; set; }
    }
}