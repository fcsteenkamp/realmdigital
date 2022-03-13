using Cronos;
using Microsoft.Extensions.Logging;
using realmdigital_workerservice.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace realmdigital_workerservice.Services.SchedulerServices
{
    public class WorkAnniversaryService : ISchedulerService
    {
        private readonly ILogger<WorkAnniversaryService> _logger;
        private readonly IEmployeeService _employeeService;
        private readonly IMailService _mailService;

        public WorkAnniversaryService(ILogger<WorkAnniversaryService> logger, IEmployeeService employeeService, IMailService mailService)
        {
            _logger = logger;
            _employeeService = employeeService;
            _mailService = mailService;
        }

        public async Task<Task> ExecuteAsync(CancellationToken stoppingToken)
        {

            _logger.LogInformation("Send Work Anniversary Worker running at: {time}", DateTimeOffset.Now);


            return Task.CompletedTask;
        }

    }
}
