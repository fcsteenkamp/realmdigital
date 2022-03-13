using realmdigital_workerservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace realmdigital_workerservice.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(Mail mail);
    }
}
