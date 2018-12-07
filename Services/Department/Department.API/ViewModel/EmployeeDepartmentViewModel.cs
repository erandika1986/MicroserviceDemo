using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Department.API.ViewModel
{
    public class EmployeeDepartmentViewModel
    {
        public EmployeeDepartmentViewModel()
        {
            DepartmentIds = new List<int>();
        }

        public int EmployeeId { get; set; }
        public List<int> DepartmentIds { get; set; }
    }
}
