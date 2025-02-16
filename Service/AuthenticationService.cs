using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
	public class AuthenticationService : IAuthenticationService
	{
		private readonly ILoggerManager loggerManager;
		private readonly IMapper mapper;
		private readonly UserManager<User> userManager;
		private readonly IConfiguration configuration;

		public AuthenticationService(ILoggerManager loggerManager, 
			IMapper mapper, 
			UserManager<User> userManager, 
			IConfiguration configuration)
		{
			this.loggerManager = loggerManager;
			this.mapper = mapper;
			this.userManager = userManager;
			this.configuration = configuration;
		}

		public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistrationDto)
		{
			var user = this.mapper.Map<User>(userForRegistrationDto);

			var result = await this.userManager.CreateAsync(user, userForRegistrationDto.Password);

			if (result.Succeeded)
			{
				await this.userManager.AddToRolesAsync(user, userForRegistrationDto.Roles);
			}

			return result;
		}
	}
}
