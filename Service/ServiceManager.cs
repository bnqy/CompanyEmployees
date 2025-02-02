using Contracts;
using Service.Contracts;
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

		public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager loggerManager)
		{
			this.companyService = new Lazy<ICompanyService>(() => new CompanyService(repositoryManager, loggerManager));
			this.employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(repositoryManager, loggerManager));
		}


		public ICompanyService CompanyService => this.companyService.Value;

		public IEmployeeService EmployeeService => this.employeeService.Value;
	}
}
