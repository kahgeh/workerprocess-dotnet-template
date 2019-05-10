using System;
using System.Collections.Generic;
using System.Linq;
using WorkerProcess.Template.Contracts;

namespace WorkerProcess.Template
{
    public class WorkerInfo
    {
        public DateTime StartedUtc { get; set; }
        public bool IsAlive { get; set; }
        public Queue<Job> PastAndCurrentJobs { get; set; }

        public WorkerInfo()
        {
            IsAlive = true;
            PastAndCurrentJobs = new Queue<Job>();
        }
    }

    public interface IWorkerInfoStore
    {
        void MarkAsNotAlive();
        void AssignStatus(JobStatus status);
        void AddJob(Job job);
    }

    public interface IWorkerInfoProvider
    {
        List<Job> GetPastAndCurrentJobs();

        bool IsAlive();
    }

    public class WorkerInfoStore : IWorkerInfoStore
    {
        public int PastAndCurrentJobsQueueSize { get; }
        private WorkerInfo _workerInfo;
        public WorkerInfoStore(WorkerInfo workerInfo, int pastAndCurrentJobsQueueSize)
        {
            _workerInfo = workerInfo;
            PastAndCurrentJobsQueueSize = pastAndCurrentJobsQueueSize;
        }

        public void MarkAsNotAlive()
        {
            _workerInfo.IsAlive = false;
        }

        public void AssignStatus(JobStatus status)
        {
            var currentJob = _workerInfo.PastAndCurrentJobs.Last();
            currentJob.EndUtc = DateTime.UtcNow;
            currentJob.ElapsedTime = currentJob.EndUtc - currentJob.StartUtc;
            currentJob.Status = status;
        }
        public void AddJob(Job job)
        {
            _workerInfo.PastAndCurrentJobs.Enqueue(job);
            if (_workerInfo.PastAndCurrentJobs.Count() > PastAndCurrentJobsQueueSize)
            {
                _workerInfo.PastAndCurrentJobs.Dequeue();
            }
        }
    }

    public class WorkerInfoProvider : IWorkerInfoProvider
    {
        private WorkerInfo _workerInfo;
        public WorkerInfoProvider(WorkerInfo workerInfo)
        {
            _workerInfo = workerInfo;
        }

        public bool IsAlive()
        {
            return _workerInfo.IsAlive;
        }

        public List<Job> GetPastAndCurrentJobs()
        {
            return _workerInfo.PastAndCurrentJobs.ToList();
        }
    }
}