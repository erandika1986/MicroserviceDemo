using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mobile.HttpAggregator.ViewModels
{
    public class EmployeeViewModel
    {
        public EmployeeViewModel()
        {
            Departments = new List<DepartmentViewModel>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }

        public List<DepartmentViewModel> Departments { get; set; }
    }
}
