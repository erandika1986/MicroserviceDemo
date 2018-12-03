using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employee.API.Infrastructure.Exceptions
{
    public class EmployeeDomainException : Exception
    {
        public EmployeeDomainException()
        { }

        public EmployeeDomainException(string message)
            : base(message)
        { }

        public EmployeeDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
