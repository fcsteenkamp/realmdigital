using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using realmdigital_workerservice.Inerfaces;
using realmdigital_workerservice.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace realmdigital_workerservice.Services.DataServices
{
    public class EmployeeService : IEmployeeService
    {
        private readonly APISettings _apiSettings;

        public EmployeeService(IOptions<APISettings> apiSettings)
        {
            _apiSettings = apiSettings.Value;
        }

        public async Task<List<int>> GetBirthdayExclusionList()
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            string result;
            using (WebClient client = new WebClient())
            {
                result = await client.DownloadStringTaskAsync(_apiSettings.BaseURL +"/do-not-send-birthday-wishes");
            }
            return JsonConvert.DeserializeObject<List<int>>(result);
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            string result;
            using (WebClient client = new WebClient())
            {
                result = await client.DownloadStringTaskAsync(_apiSettings.BaseURL + "/employees");
            }

            return JsonConvert.DeserializeObject<IEnumerable<Employee>>(result);
        }

        public async Task UpdateEmployee(Employee employee)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            using (var webClient = new WebClient())
            {
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                await webClient.UploadStringTaskAsync(_apiSettings.BaseURL + "/employees/" + employee.EmployeeId.ToString(),
                    WebRequestMethods.Http.Put,
                    JsonConvert.SerializeObject(employee));
            }
        }
    }
}
