using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.Controllers
{
	[Route("api/companies/{companyId}/employees")]
	[ApiController]
	public class EmployeesController : ControllerBase
	{
		private readonly IServiceManager serviceManager;

		public EmployeesController(IServiceManager serviceManager)
		{
			this.serviceManager = serviceManager;
		}

		[HttpGet] // companyId -- will be mapped from the main above route. So we do not need to specify here.
		public IActionResult GetEmployeesForCompany(Guid companyId)
		{
			var employees = this.serviceManager.EmployeeService.GetEmployees(companyId, false);

			return Ok(employees);
		}

		[HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
		public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)
		{
			var employee = this.serviceManager.EmployeeService.GetEmployee(companyId, id, false);

			return Ok(employee);
		}

		[HttpPost]
		public IActionResult CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employeeForCreationDto)
		{
			if (employeeForCreationDto == null)
			{
				return BadRequest("EmployeeForCreationDto is null.");
			}

			var employeeDto = this.serviceManager.EmployeeService.CreateEmployeeForCompany(companyId, employeeForCreationDto, false);

			return CreatedAtRoute("GetEmployeeForCompany",  new { companyId, id = employeeDto.Id}, employeeDto);
		}

		[HttpDelete("{id:guid}")]
		public IActionResult DeleteEmployeeForCompany(Guid companyId, Guid id)
		{
			this.serviceManager.EmployeeService.DeleteEmployeeForCompany(companyId, id, false);

			return NoContent();
		}

		[HttpPut("{id:guid}")]
		public IActionResult UpdateEmployeeForCompany(Guid companyId,
			Guid id, 
			[FromBody] EmployeeForUpdateDto employeeForUpdateDto)
		{
			if (employeeForUpdateDto is null)
			{
				return BadRequest("EmployeeForUpdateDto is null.");
			}

			this.serviceManager.EmployeeService.UpdateEmployeeForCompany(companyId, id, employeeForUpdateDto, false, true);

			return NoContent();
		}

		[HttpPatch("{id:guid}")]
		public IActionResult PatchingUpdateEmployeeForCompany(Guid companyId,
			Guid id,
			[FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDocument)
		{
			if (patchDocument is null)
			{
				return BadRequest("JsonPatchDocument is null.");
			}

			var result = this.serviceManager.EmployeeService.GetEmployeeForPatch(companyId, id, false, true);

			patchDocument.ApplyTo(result.employeeForPatch);

			this.serviceManager.EmployeeService.SaveChangesForPatch(result.employeeForPatch, result.employee);

			return NoContent();
		}
	}
}
