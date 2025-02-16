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
	public sealed class ServiceManager : IServiceManager
	{
		private readonly Lazy<ICompanyService> companyService;
		private readonly Lazy<IEmployeeService> employeeService;
		private readonly Lazy<IAuthenticationService> authenticationService;

		public ServiceManager(IRepositoryManager repositoryManager,
			ILoggerManager loggerManager,
			IMapper mapper,
			IEmployeeLinks employeeLinks,
			UserManager<User> userManager,
			IConfiguration configuration)
		{
			this.companyService = new Lazy<ICompanyService>(() => new CompanyService(repositoryManager, loggerManager, mapper));
			this.employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(repositoryManager, loggerManager, mapper, employeeLinks));
			this.authenticationService = new Lazy<IAuthenticationService>(() =>
			new AuthenticationService(loggerManager, mapper, userManager,configuration));
		}


		public ICompanyService CompanyService => this.companyService.Value;

		public IEmployeeService EmployeeService => this.employeeService.Value;

		public IAuthenticationService AuthenticationService => this.authenticationService.Value;
	}
}
