using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using realmdigital_workerservice.Inerfaces;
using realmdigital_workerservice.Models;
using realmdigital_workerservice.Services.DataServices;
using realmdigital_workerservice.Services.UtilityServices;
using realmdigital_workerservice.Services.SchedulerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace realmdigital_workerservice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;
                    services.Configure<APISettings>(configuration.GetSection("API"));
                    services.Configure<MailSettings>(configuration.GetSection("MailSettings"));

                    services.AddHostedService<Worker>();

                    services.AddScoped<IEmployeeService, EmployeeService>();

                    services.AddScoped<ISchedulerService, BirthdayWishesService>();
                    services.AddScoped<ISchedulerService, WorkAnniversaryService>();

                    services.AddScoped<IMailService, MailService>();
                });
    }
}
