using Mobile.HttpAggregator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mobile.HttpAggregator.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<EmployeeModel> GetEmployeeById(int id);
        Task<EmployeeModel> CreateEmployee(EmployeeModel employee);
    }
}
