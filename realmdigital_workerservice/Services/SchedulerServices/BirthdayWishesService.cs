using Cronos;
using Microsoft.Extensions.Logging;
using realmdigital_workerservice.Inerfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace realmdigital_workerservice.Services.SchedulerServices
{
    public class BirthdayWishesService : ISchedulerService
    {
        private readonly ILogger<BirthdayWishesService> _logger;
        private readonly IEmployeeService _employeeService;
        private readonly IMailService _mailService;
        public BirthdayWishesService(ILogger<BirthdayWishesService> logger, IEmployeeService employeeService, IMailService mailService)
        {
            _logger = logger;
            _employeeService = employeeService;
            _mailService = mailService;
        }

        public async Task<Task> ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Send Birthday Wishes Worker running at: {time}", DateTimeOffset.Now);

            var qBirthdayExclusions = _employeeService.GetBirthdayExclusionList().Result;

            bool isLeapDay = DateTime.IsLeapYear(DateTime.Now.Year) && DateTime.Now.Month == 2 && DateTime.Now.Day == 28; //Tomorrow should have been my birthday, so lets celebrate today?

            var qEmployeeWithBirthDays = _employeeService.GetEmployees().Result.Where(o => (o.EmploymentStartDate.HasValue && o.EmploymentStartDate.Value.Date <= DateTime.Now) //Should be an employee that has started
                                                                                    && !qBirthdayExclusions.Contains(o.EmployeeId) //Birthday exclusions
                                                                                    && (o.LastNotification == null || o.LastNotification.Value.Year < DateTime.Now.Year) //Check last notification date (not to spam)
                                                                                    && o.DateOfBirth.HasValue
                                                                                    && (!o.EmploymentEndDate.HasValue || o.EmploymentEndDate.Value >= DateTime.Now) //Check if employee is still employed at realm
                                                                                    && ((o.DateOfBirth.Value.Month == DateTime.Now.Month && o.DateOfBirth.Value.Day == DateTime.Now.Day)
                                                                                    && (!isLeapDay || (o.DateOfBirth.Value.Month == DateTime.Now.Month && o.DateOfBirth.Value.Day == 29)))).ToList(); //Include leap year employees if applicable

            foreach (var employee in qEmployeeWithBirthDays)
            {
                try
                {
                    //Send Email
                    await _mailService.SendEmailAsync(new Models.Mail()
                    {
                        Subject = "Happy Birthday!",
                        Body = $"Happy Birthday <b>{employee.Name} {employee.Lastname}</b> <br /><br /> You were born {GetAge((DateTime)employee.DateOfBirth)} years ago on <b>{employee.DateOfBirth.Value.ToString("dd MMMM yyyy")}</b>"
                    });

                    _logger.LogWarning("Happy Brithday to: ({Id}) {Name} {Surname}. Born on {DateOfBirth}",employee.EmployeeId, employee.Name,employee.Lastname, employee.DateOfBirth.Value.ToString("dd MMMM yyyy"));

                    //Update last sent
                    employee.LastNotification = DateTime.Now;
                    await _employeeService.UpdateEmployee(employee);

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.GetBaseException().Message);
                }

            }

            _logger.LogInformation("Employees with birthdays: {count}", qEmployeeWithBirthDays.Count());


            return Task.CompletedTask;
        }

        private int GetAge(DateTime dob)
        {
            // Today's date.
            var today = DateTime.Today;

            // Calculate the age.
            var age = today.Year - dob.Year;

            // Go back to the year in which the person was born in case of a leap year
            if (dob.Date > today.AddYears(-age)) age--;

            return age;
        }

    }
}
