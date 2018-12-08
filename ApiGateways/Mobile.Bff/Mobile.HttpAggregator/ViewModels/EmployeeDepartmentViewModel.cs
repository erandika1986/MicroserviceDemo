using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mobile.HttpAggregator.ViewModels
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
