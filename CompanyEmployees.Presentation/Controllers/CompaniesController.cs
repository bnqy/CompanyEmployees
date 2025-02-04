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
	[Route("api/companies")]
	[ApiController]
	public class CompaniesController : ControllerBase
	{
		private readonly IServiceManager serviceManager;

		public CompaniesController(IServiceManager serviceManager)
		{
			this.serviceManager = serviceManager;
		}

		[HttpGet]
		public IActionResult GetCompanies() // Since there is no [Route] attr. this's route is api/companies. IActioResult returns result + status code.
		{
			var companies = this.serviceManager.CompanyService.GetAllCompanies(false);

			return Ok(companies);
		}

		[HttpGet("{id:guid}", Name = "CompanyById")]
		public IActionResult GetCompany(Guid id) // /api/companies/id
		{
			var company = this.serviceManager.CompanyService.GetCompany(id, false);

			return Ok(company);
		}

		[HttpPost]
		public IActionResult CreateCompany([FromBody] CompanyForCreationDto companyForCreationDto)
		{
			if (companyForCreationDto is null)
			{
				return this.BadRequest("CompanyForCreationDto is null.");
			}

			var addedCompany = this.serviceManager.CompanyService.CreateCompany(companyForCreationDto);

			return this.CreatedAtRoute("CompanyById", new { id = addedCompany.Id }, addedCompany);
		}
	}
}
