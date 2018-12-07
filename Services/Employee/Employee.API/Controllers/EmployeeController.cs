using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Employee.API.Data;
using Employee.API.Models;
using Employee.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Employee.API.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeContext _employeeContext;
        private readonly EmpoyeeSettings _settings;

        public EmployeeController(EmployeeContext context, IOptionsSnapshot<EmpoyeeSettings> settings)
        {
            this._employeeContext = context ?? throw new ArgumentNullException(nameof(context));
            this._settings = settings.Value;
            ((DbContext)context).ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [Authorize]
        [HttpGet]
        [Route("departments")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<EmployeeModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<EmployeeModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Departments([FromQuery] int pageSize = 10, [FromQuery]int pageIndex = 0, [FromQuery] string ids = null)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                return GetItemsByIds(ids);
            }

            var totalEmployees = await _employeeContext.Employees.LongCountAsync();

            var employeesOnPage = await _employeeContext.Employees
                .OrderBy(c => c.FirstName)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            var model = new PaginatedItemsViewModel<EmployeeModel>(
                pageIndex, pageSize, totalEmployees, employeesOnPage);

            return Ok(model);
        }

        [Authorize]
        [HttpGet]
        [Route("employee/{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(EmployeeModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var employee = await _employeeContext.Employees.SingleOrDefaultAsync(d => d.Id == id);

            if (employee != null)
            {
                return Ok(employee);
            }

            return NotFound();
        }




        [Authorize]
        [HttpPost]
        [Route("employee")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateEmployee([FromBody]EmployeeModel employee)
        {
            var item = new EmployeeModel()
            {
                IsActive = true,
                Email = employee.Email,
                FirstName=employee.FirstName,
                LastName=employee.LastName
            };

            _employeeContext.Employees.Add(item);

            await _employeeContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployeeById), new { id = item.Id }, null);
        }



        [Authorize]
        [Route("id")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employeee = _employeeContext.Employees.SingleOrDefault(d => d.Id == id);

            if (employeee == null)
            {
                return NotFound();
            }



            _employeeContext.Employees.Remove(employeee);

            await _employeeContext.SaveChangesAsync();

            return NoContent();
        }

        private IActionResult GetItemsByIds(string ids)
        {
            var numIds = ids.Split(',')
                .Select(id => (Ok: int.TryParse(id, out int x), Value: x));

            if (!numIds.All(nid => nid.Ok))
            {
                return BadRequest("ids value invalid. Must be comma-separated list of numbers");
            }

            var idsToSelect = numIds
                .Select(id => id.Value);

            var items = _employeeContext.Employees.Where(ci => idsToSelect.Contains(ci.Id)).ToList();

            return Ok(items);
        }
    }
}