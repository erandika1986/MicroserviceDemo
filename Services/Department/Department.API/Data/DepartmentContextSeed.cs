using Department.API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Department.API.Data
{
    public class DepartmentContextSeed
    {
        public async Task SeedAsync(DepartmentContext context, IHostingEnvironment env, IOptions<DepartmentSettings> settings, ILogger<DepartmentContextSeed> logger)
        {
            if (!context.Departments.Any())
            {
                context.Departments.AddRange(GetPreconfiguredDepartment());
                await context.SaveChangesAsync();
            }
        }

        static IEnumerable<DepartmentModel> GetPreconfiguredDepartment()
        {
            return new List<DepartmentModel>()
            {
                new DepartmentModel() { Name="HR",IsActive=true },
                new DepartmentModel() { Name="Finance",IsActive=true },
                new DepartmentModel() { Name="Engineering",IsActive=true }
            };
        }
    }
}
