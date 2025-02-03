using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
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
	}
}
