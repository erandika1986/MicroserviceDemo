using Mobile.HttpAggregator.Models;
using Mobile.HttpAggregator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mobile.HttpAggregator.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentModel>>  GetEmployeeAssignedDepartment(int employeeid);
        Task<ResposeViewModel> AssignEmployeeDepartments(EmployeeDepartmentViewModel vm);
    }
}
