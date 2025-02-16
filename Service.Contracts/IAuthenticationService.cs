﻿using Microsoft.AspNetCore.Identity;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
	public interface IAuthenticationService
	{
		// Executes registration logis. Returns identity result.
		Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistrationDto);
		Task<bool> ValidateUser(UserForAuthenticationDto userForAuthenticationDto);
		Task<TokenDto> CreateToken(bool populateExp);
		Task<TokenDto> RefreshToken(TokenDto tokenDto);
	}
}
