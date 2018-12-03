using Employee.API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employee.API.Data
{
    public class EmployeeContextSeed
    {

        public async Task SeedAsync(EmployeeContext context, IHostingEnvironment env, IOptions<EmpoyeeSettings> settings, ILogger<EmployeeContextSeed> logger)
        {
            if (!context.Employees.Any())
            {
                context.Employees.AddRange(GetPreconfiguredEmployee());
                await context.SaveChangesAsync();
            }
        }

        static IEnumerable<EmployeeModel> GetPreconfiguredEmployee()
        {
            return new List<EmployeeModel>()
            {
                new EmployeeModel() {Email="erandika1986@gmail.com",FirstName="Erandika",LastName="Sandaruwan",IsActive=true },
                new EmployeeModel() {Email="piumika.sujani@gmail.com",FirstName="Sujani",LastName="Piumika",IsActive=true },
                new EmployeeModel() {Email="sudeepa@gmail.com",FirstName="Sudeepa",LastName="Madushanka",IsActive=true }
            };
        }
    }
}
