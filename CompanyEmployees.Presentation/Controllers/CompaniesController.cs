﻿using CompanyEmployees.Presentation.ActionFilters;
using CompanyEmployees.Presentation.ModelBinders;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.Controllers
{
	[ApiExplorerSettings(GroupName = "v1")]
	[Route("api/companies")]
	[ApiController]
	//[ResponseCache(CacheProfileName = "60SecondsDuration")]  // Marvin.Cache.Headers will provide this.
	public class CompaniesController : ControllerBase
	{
		private readonly IServiceManager serviceManager;

		public CompaniesController(IServiceManager serviceManager)
		{
			this.serviceManager = serviceManager;
		}

		/// <summary>
		/// Gets list of all companies.
		/// </summary>
		/// <returns>The companies list.</returns>
		[HttpGet(Name = "GetCompanies")]
		[Authorize(Roles = "Manager")]
		public async Task<IActionResult> GetCompanies() // Since there is no [Route] attr. this's route is api/companies. IActioResult returns result + status code.
		{
			var companies = await this.serviceManager.CompanyService.GetAllCompaniesAsync(false);

			return Ok(companies);
		}

		[HttpGet("{id:guid}", Name = "CompanyById")]
		//[ResponseCache(CacheProfileName = "60SecondsDuration")]
		[HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 80)]
		[HttpCacheValidation(MustRevalidate = false)]
		public async Task<IActionResult> GetCompany(Guid id) // /api/companies/id
		{
			var company = await this.serviceManager.CompanyService.GetCompanyAsync(id, false);

			return Ok(company);
		}

		[HttpGet("collection/({ids})", Name = "CompanyCollection")]
		public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
		{
			var companies = await this.serviceManager.CompanyService.GetByIdsAsync(ids, false);

			return Ok(companies);
		}

		[HttpPost(Name = "CreateCompany")]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto companyForCreationDto)
		{
			var addedCompany = await this.serviceManager.CompanyService.CreateCompanyAsync(companyForCreationDto);

			return this.CreatedAtRoute("CompanyById", new { id = addedCompany.Id }, addedCompany); // CreatedAtRoute returns 201.
		}

		[HttpPost("collection")]
		public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyForCreationDtos)
		{
			var companiesDto = await this.serviceManager.CompanyService.CreateCompanyCollectionAsync(companyForCreationDtos);

			return CreatedAtRoute("CompanyCollection", new { companiesDto.ids }, companiesDto.companies);
		}

		[HttpDelete("{id:guid}")]
		public async Task<IActionResult> DeleteCompany(Guid id)
		{
			await this.serviceManager.CompanyService.DeleteCompanyAsync(id, false);

			return NoContent();
		}

		[HttpPut("{id:guid}")]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		public async Task<IActionResult> UpdateCompany(Guid id, 
			[FromBody] CompanyForUpdateDto companyForUpdateDto)
		{
			await this.serviceManager.CompanyService.UpdateCompanyAsync(id, companyForUpdateDto, true);

			return NoContent();
		}

		[HttpOptions]
		public IActionResult GetCompaniesOptions()
		{
			Response.Headers.Add("Allow", "GET, OPTIONS, POST, PUT, DELETE");

			return Ok();
		}
	}
}
