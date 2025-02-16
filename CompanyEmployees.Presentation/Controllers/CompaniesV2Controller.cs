using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.Controllers
{
	[ApiExplorerSettings(GroupName = "v2")]
	[Route("api/companies")]
	[ApiController]
	public class CompaniesV2Controller : ControllerBase
	{
		private readonly IServiceManager serviceManager;

		public CompaniesV2Controller(IServiceManager serviceManager)
		{
			this.serviceManager = serviceManager;
		}

		public async Task<IActionResult> GetCompanies()
		{
			var companies = await this.serviceManager.CompanyService.GetAllCompaniesAsync(false);

			var companies2 = companies.Select(x => $"{x.Name} V2");

			return Ok(companies2);
		}
	}
}
