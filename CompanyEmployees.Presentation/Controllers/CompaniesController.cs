using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
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

		[HttpGet("{id:guid}")]
		public IActionResult GetCompany(Guid id) // /api/companies/id
		{
			var company = this.serviceManager.CompanyService.GetCompany(id, false);

			return Ok(company);
		}
	}
}
