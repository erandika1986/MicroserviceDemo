using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mobile.HttpAggregator.Config;
using Mobile.HttpAggregator.Models;
using Mobile.HttpAggregator.Services.Interfaces;
using Mobile.HttpAggregator.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mobile.HttpAggregator.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<DepartmentService> _logger;
        private readonly UrlsConfig _urls;

        public DepartmentService(HttpClient httpClient, ILogger<DepartmentService> logger, IOptions<UrlsConfig> config)
        {
            _httpClient = httpClient;
            _logger = logger;
            _urls = config.Value;
        }

        public async Task<ResposeViewModel> AssignEmployeeDepartments(EmployeeDepartmentViewModel vm)
        {
            var empoyeeeDepartment = new StringContent(JsonConvert.SerializeObject(vm), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_urls.Department + UrlsConfig.DepartmentOperations.AssignEmployeeDepartments(), empoyeeeDepartment);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();


            return JsonConvert.DeserializeObject<ResposeViewModel>(responseString);
        }

        public async  Task<IEnumerable<DepartmentModel>> GetEmployeeAssignedDepartment(int employeeid)
        {
            var data = await _httpClient.GetStringAsync(_urls.Department + UrlsConfig.DepartmentOperations.GetEmployeeAssignedDepartment(employeeid));

            var departments = !string.IsNullOrEmpty(data) ? JsonConvert.DeserializeObject<IEnumerable<DepartmentModel>>(data) : null;

            return departments;
        }
    }
}
