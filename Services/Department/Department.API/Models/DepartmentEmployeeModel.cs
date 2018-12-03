using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Department.API.Models
{
    public class DepartmentEmployeeModel
    {
        public int DepartmentId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime AssignedDate { get; set; }
        public bool IsActive { get; set; }

        public virtual DepartmentModel Department { get; set; }
    }
}
