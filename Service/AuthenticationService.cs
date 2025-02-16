using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
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

		private User? user;

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

		public async Task<TokenDto> CreateToken(bool populateExp)
		{
			var signingCredentials = GetSigningCredentials();
			var claims = await GetClaims();
			var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

			var refreshToken = GenerateRefreshToken();

			user.RefreshToken = refreshToken;

			if (populateExp)
				user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

			await userManager.UpdateAsync(user);

			var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

			return new TokenDto(accessToken, refreshToken);
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

		// Check if user exists and password matches.
		public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuthenticationDto)
		{
			this.user = await this.userManager.FindByNameAsync(userForAuthenticationDto.UserName);

			var result = (this.user != null && await this.userManager.CheckPasswordAsync(this.user, userForAuthenticationDto.Password));

			if (!result)
			{
				this.loggerManager.LogWarn($"{nameof(ValidateUser)}: Authentication failed. Wrong user name or password.");
			}

			return result;
		}

		// Returns secret key as a byte array with the security algorithm
		private SigningCredentials GetSigningCredentials()
		{
			var jwtSettings = this.configuration.GetSection("JwtSettings");
			var keyFromJWTSettins = jwtSettings["key"];
			var key = Encoding.UTF8.GetBytes(keyFromJWTSettins);
			var secret = new SymmetricSecurityKey(key);

			return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
		}

		// List of claims with user name inside and all roles.
		private async Task<List<Claim>> GetClaims()
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, this.user.UserName)
			};

			var roles = await this.userManager.GetRolesAsync(this.user);

			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}

			return claims;
		}

		private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials,
			List<Claim> claims)
		{
			var jwtSettings = this.configuration.GetSection("JwtSettings");

			var tokenOptions = new JwtSecurityToken 
			(
				issuer: jwtSettings["validIssuer"],
				audience: jwtSettings["validAudience"],
				claims: claims,
				expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
				signingCredentials: signingCredentials
			);

			return tokenOptions;
		}

		private string GenerateRefreshToken()
		{
			var randomNumber = new byte[32];

			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(randomNumber);
				return Convert.ToBase64String(randomNumber);
			}
		}

		private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
		{
			var jwtSettings = configuration.GetSection("JwtSettings");

			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateAudience = true,
				ValidateIssuer = true,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(
					Encoding.UTF8.GetBytes(jwtSettings["key"])),
				ValidateLifetime = true,
				ValidIssuer = jwtSettings["validIssuer"],
				ValidAudience = jwtSettings["validAudience"]
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			SecurityToken securityToken;
			var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out
		securityToken);

			var jwtSecurityToken = securityToken as JwtSecurityToken;
			if (jwtSecurityToken == null ||
		!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
				StringComparison.InvariantCultureIgnoreCase))
			{
				throw new SecurityTokenException("Invalid token");
			}

			return principal;
		}

		public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
		{
			var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);

			var user1 = await userManager.FindByNameAsync(principal.Identity.Name);

			if (user1 == null 
				|| user1.RefreshToken != tokenDto.RefreshToken 
				|| user1.RefreshTokenExpiryTime <= DateTime.Now)

				throw new RefreshTokenBadRequest();

			user = user1;

			return await CreateToken(populateExp: false);
		}
	}
}
