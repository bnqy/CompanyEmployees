using CompanyEmployees.Presentation.ActionFilters;
using Entities.LinkModels;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
		[ServiceFilter(typeof(ValidateMediaTypeAttribute))]
		public async Task<IActionResult> GetEmployeesForCompany(Guid companyId,
			[FromQuery] EmployeeParameters employeeParameters) // query params will be used.
		{
			var linkParams = new LinkParameters(employeeParameters, HttpContext);

			var employeesPagedResult = await this.serviceManager.EmployeeService.GetEmployeesAsync(
				companyId, linkParams, false);

			Response.Headers.Add("X-Pagination",
				JsonSerializer.Serialize(employeesPagedResult.metaData));

			return employeesPagedResult.linkResponse.HasLinks ? Ok(employeesPagedResult.linkResponse.LinkedEntities) :
				Ok(employeesPagedResult.linkResponse.ShapedEntities);
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
