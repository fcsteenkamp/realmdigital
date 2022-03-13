using realmdigital_workerservice.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realmdigital_workerservice.Inerfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetEmployees();
        Task<List<int>> GetBirthdayExclusionList();
        Task UpdateEmployee(Employee employee);
    }
}
