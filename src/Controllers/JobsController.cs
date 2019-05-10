using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using WorkerProcess.Template.Contracts;

namespace WorkerProcess.Template
{
    [Route("[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IWorkerInfoProvider _workerInfoProvider;
        public JobsController(IWorkerInfoProvider workerInfoProvider)
        {
            _workerInfoProvider = workerInfoProvider;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(
                _workerInfoProvider.GetPastAndCurrentJobs());
        }
    }
}