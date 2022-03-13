using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace realmdigital_workerservice.Interfaces
{
    public interface ISchedulerService
    {
        Task<Task> ExecuteAsync(CancellationToken cancellationToken);
    }
}
