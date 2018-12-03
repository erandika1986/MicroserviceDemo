using Department.API.Data.EntityConfigurations;
using Department.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Department.API.Data
{
    public class DepartmentContext : DbContext
    {
        public DepartmentContext(DbContextOptions<DepartmentContext> options) : base(options)
        {

        }

        public DbSet<DepartmentModel> Departments { get; set; }
        public DbSet<DepartmentEmployeeModel> DepartmentEmployees { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new DepartmentConfiguration());
            builder.ApplyConfiguration(new DepartmentEmployeeConfiguration());
        }
    }
}
