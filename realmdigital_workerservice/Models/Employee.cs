using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace realmdigital_workerservice.Models
{
    public partial class Employee
    {
        [JsonProperty("id")]
        public int EmployeeId { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("lastname", NullValueHandling = NullValueHandling.Ignore)]
        public string Lastname { get; set; }

        [JsonProperty("dateOfBirth", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? DateOfBirth { get; set; }

        [JsonProperty("employmentStartDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? EmploymentStartDate { get; set; }

        [JsonProperty("employmentEndDate")]
        public DateTime? EmploymentEndDate { get; set; }

        [JsonProperty("lastNotification", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? LastNotification { get; set; }

    }
}
