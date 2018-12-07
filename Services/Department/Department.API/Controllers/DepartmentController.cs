using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Department.API.Data;
using Department.API.Models;
using Department.API.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Department.API.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly DepartmentContext _departmentContext;
        private readonly DepartmentSettings _settings;

        public DepartmentController(DepartmentContext context, IOptionsSnapshot<DepartmentSettings> settings)
        {
            this._departmentContext = context ?? throw new ArgumentNullException(nameof(context));
            this._settings = settings.Value;
            ((DbContext)context).ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [Authorize]
        [HttpGet]
        [Route("departments")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<DepartmentModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<DepartmentModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Departments([FromQuery] int pageSize = 10, [FromQuery]int pageIndex = 0, [FromQuery] string ids = null)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                return GetItemsByIds(ids);
            }

            var totalDepartments = await _departmentContext.Departments.LongCountAsync();

            var departmentsOnPage = await _departmentContext.Departments
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            var model = new PaginatedItemsViewModel<DepartmentModel>(
                pageIndex, pageSize, totalDepartments, departmentsOnPage);

            return Ok(model);
        }

        [Authorize]
        [HttpGet]
        [Route("department/{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(DepartmentModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var department = await _departmentContext.Departments.SingleOrDefaultAsync(d => d.Id == id);

            if (department != null)
            {
                return Ok(department);
            }

            return NotFound();
        }

        [Authorize]
        [HttpGet]
        [Route("employeedepartments/{employeeid:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<DepartmentModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetEmployeeAssignedDepartment(int employeeid)
        {
            var departments = await _departmentContext.DepartmentEmployees.Where(t => t.EmployeeId == employeeid).Select(t => t.Department).ToListAsync();

            return Ok(departments);
        }



        [Authorize]
        [HttpPost]
        [Route("department")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateDepartment([FromBody]DepartmentModel department)
        {
            var item = new DepartmentModel()
            {
                IsActive = true,
                Name = department.Name
            };

            _departmentContext.Departments.Add(item);

            await _departmentContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDepartmentById), new { id = item.Id }, null);
        }

        [Authorize]
        [HttpPost]
        [Route("assignemployeedepartments")]
        [ProducesResponseType(typeof(ResposeViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AssignEmployeeDepartments([FromBody]EmployeeDepartmentViewModel vm)
        {
            var response = new ResposeViewModel();

            try
            {
                foreach (var deptId in vm.DepartmentIds)
                {
                    var ed = new DepartmentEmployeeModel()
                    {
                        DepartmentId = deptId,
                        EmployeeId = vm.EmployeeId,
                        AssignedDate = DateTime.UtcNow,
                        IsActive=true
                    };

                    _departmentContext.Add(ed);

                    await _departmentContext.SaveChangesAsync();
                }

                response.IsSuccess = true;
            }
            catch(Exception ex)
            {
                response.Message = ex.ToString();
            }

            return Ok(response);
        }



        [Authorize]
        [Route("id")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = _departmentContext.Departments.SingleOrDefault(d => d.Id == id);

            if (department == null)
            {
                return NotFound();
            }

            foreach (var de in department.DepartmentEmployees)
            {
                _departmentContext.DepartmentEmployees.Remove(de);
            }

            _departmentContext.Departments.Remove(department);

            await _departmentContext.SaveChangesAsync();

            return NoContent();
        }


        private IActionResult GetItemsByIds(string ids)
        {
            throw new NotImplementedException();
        }
    }
}