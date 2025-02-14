using CompanyEmployees.Presentation.ActionFilters;
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
		public async Task<IActionResult> GetEmployeesForCompany(Guid companyId)
		{
			var employees = await this.serviceManager.EmployeeService.GetEmployeesAsync(companyId, false);

			return Ok(employees);
		}

		[HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
		public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid id)
		{
			var employee = await this.serviceManager.EmployeeService.GetEmployeeAsync(companyId, id, false);

			return Ok(employee);
		}

		[HttpPost]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employeeForCreationDto)
		{
			/*if (employeeForCreationDto == null)
			{
				return BadRequest("EmployeeForCreationDto is null.");
			}*/

			/*if (!ModelState.IsValid)
			{
				return UnprocessableEntity(ModelState);
			}*/

			var employeeDto = await this.serviceManager.EmployeeService.CreateEmployeeForCompanyAsync(companyId, employeeForCreationDto, false);

			return CreatedAtRoute("GetEmployeeForCompany",  new { companyId, id = employeeDto.Id}, employeeDto);
		}

		[HttpDelete("{id:guid}")]
		public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid id)
		{
			await this.serviceManager.EmployeeService.DeleteEmployeeForCompanyAsync(companyId, id, false);

			return NoContent();
		}

		[HttpPut("{id:guid}")]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId,
			Guid id, 
			[FromBody] EmployeeForUpdateDto employeeForUpdateDto)
		{
			/*if (employeeForUpdateDto is null)
			{
				return BadRequest("EmployeeForUpdateDto is null.");
			}*/

			/*if (!ModelState.IsValid)
			{
				return UnprocessableEntity(ModelState);
			}*/

			await this.serviceManager.EmployeeService.UpdateEmployeeForCompanyAsync(companyId, id, employeeForUpdateDto, false, true);

			return NoContent();
		}

		[HttpPatch("{id:guid}")]
		public async Task<IActionResult> PatchingUpdateEmployeeForCompany(Guid companyId,
			Guid id,
			[FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDocument)
		{
			if (patchDocument is null)
			{
				return BadRequest("JsonPatchDocument is null.");
			}

			var result = await this.serviceManager.EmployeeService.GetEmployeeForPatchAsync(companyId, id, false, true);

			patchDocument.ApplyTo(result.employeeForPatch, ModelState);

			TryValidateModel(result.employeeForPatch);

			if (!ModelState.IsValid)
			{
				return UnprocessableEntity(ModelState);
			}

			await this.serviceManager.EmployeeService.SaveChangesForPatchAsync(result.employeeForPatch, result.employee);

			return NoContent();
		}
	}
}
