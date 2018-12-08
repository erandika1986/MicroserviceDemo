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
    public class EmployeeService : IEmployeeService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<EmployeeService> _logger;
        private readonly UrlsConfig _urls;

        public EmployeeService(HttpClient httpClient, ILogger<EmployeeService> logger, IOptions<UrlsConfig> config)
        {
            _httpClient = httpClient;
            _logger = logger;
            _urls = config.Value;
        }

        public async Task<EmployeeModel> CreateEmployee(EmployeeModel employee)
        {
            var employeeContent = new StringContent(JsonConvert.SerializeObject(employee), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_urls.Employee + UrlsConfig.EmployeeOperations.CreateEmployee(), employeeContent);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<EmployeeModel>(responseString);
        }

        public async Task<EmployeeModel> GetEmployeeById(int id)
        {
            var data = await _httpClient.GetStringAsync(_urls.Employee + UrlsConfig.EmployeeOperations.GetEmployeeById(id));

            var employee = !string.IsNullOrEmpty(data) ? JsonConvert.DeserializeObject<EmployeeModel>(data) : null;

            return employee;
        }
    }
}
