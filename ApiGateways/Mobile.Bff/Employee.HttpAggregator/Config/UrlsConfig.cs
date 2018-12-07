using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employee.HttpAggregator.Config
{
    public class UrlsConfig
    {
        public class EmployeeOperations
        {
            public static string GetEmployeeById(int id) => $"/api/v1/Employee/employeedepartments/{id}";
            public static string CreateEmployee() => $"/api/v1/Employee/employee";
        }

        public class DepartmentOperations
        {
            public static string GetEmployeeAssignedDepartment(int employeeid) => $"/api/v1/Department/employeedepartments/{employeeid}";
            public static string AssignEmployeeDepartments() => $"/api/v1/Department/assignemployeedepartments";
        }

        public string Employee { get; set; }
        public string Department { get; set; }
    }
}
