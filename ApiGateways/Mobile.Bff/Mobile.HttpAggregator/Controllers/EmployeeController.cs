using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mobile.HttpAggregator.Models;
using Mobile.HttpAggregator.Services.Interfaces;
using Mobile.HttpAggregator.ViewModels;

namespace Mobile.HttpAggregator.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IDepartmentService departmentService, IEmployeeService employeeService)
        {
            this._departmentService = departmentService;
            this._employeeService = employeeService;
        }


        [HttpPost]
        [Route("addemployee")]
        [Authorize]
        [ProducesResponseType(typeof(EmployeeViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddNewEmployee([FromBody] EmployeeViewModel emp)
        {
            if(emp==null)
            {
                return BadRequest("EMployee cann't be null");
            }

            if (emp.Departments == null || !emp.Departments.Any())
            {
                return BadRequest("Employee must assigned to at least one department");
            }

            var newEmp = new EmployeeModel()
            {
                Email = emp.Email,
                FirstName = emp.FirstName,
                IsActive = emp.IsActive,
                LastName = emp.LastName
            };

            newEmp = await _employeeService.CreateEmployee(newEmp);

            var empDeptVm = new EmployeeDepartmentViewModel()
            {
                EmployeeId = newEmp.Id,
                DepartmentIds = emp.Departments.Select(d => d.Id).ToList()
            };

            var response = await _departmentService.AssignEmployeeDepartments(empDeptVm);

            emp.Id = newEmp.Id;

            return Ok(emp);
        }

        [HttpGet]
        [Route("getemployeedetails/{id:int}")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(EmployeeViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetEmployeeDetails(int id)
        {
            var emp = await _employeeService.GetEmployeeById(id);

            if(emp==null)
            {
                NotFound();
            }

            var empVm = new EmployeeViewModel()
            {
                Email = emp.Email,
                FirstName = emp.FirstName,
                Id = emp.Id,
                IsActive = emp.IsActive,
                LastName = emp.LastName
            };

            var depts = await _departmentService.GetEmployeeAssignedDepartment(id);

            foreach(var dept in depts)
            {
                empVm.Departments.Add(
                    new DepartmentViewModel()
                    {
                        Id = dept.Id,
                        IsActive = dept.IsActive,
                        Name = dept.Name
                    });
            }

            return Ok(empVm);
        }


    }
}