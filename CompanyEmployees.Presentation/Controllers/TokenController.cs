using CompanyEmployees.Presentation.ActionFilters;
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
	[ApiController]
	[Route("api/token")]
	public class TokenController : ControllerBase
	{
		private readonly IServiceManager serviceManager;

		public TokenController(IServiceManager serviceManager)
		{
			this.serviceManager = serviceManager;
		}

		[HttpPost("refresh")]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
		{
			var tokenDtoToReturn = await
			serviceManager.AuthenticationService.RefreshToken(tokenDto);
			return Ok(tokenDtoToReturn);
		}
	}
}
