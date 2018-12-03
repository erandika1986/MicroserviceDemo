using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Department.API.Data;
using Department.API.Models;
using Department.API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Department.API.Controllers
{
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

            if(department!=null)
            {
                return Ok(department);
            }

            return NotFound();
        }



        private IActionResult GetItemsByIds(string ids)
        {
            throw new NotImplementedException();
        }
    }
}