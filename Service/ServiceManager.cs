using AutoMapper;
using Contracts;
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

		public ServiceManager(IRepositoryManager repositoryManager,
			ILoggerManager loggerManager,
			IMapper mapper,
			IDataShaper<EmployeeDto> dataShaper)
		{
			this.companyService = new Lazy<ICompanyService>(() => new CompanyService(repositoryManager, loggerManager, mapper));
			this.employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(repositoryManager, loggerManager, mapper, dataShaper));
		}


		public ICompanyService CompanyService => this.companyService.Value;

		public IEmployeeService EmployeeService => this.employeeService.Value;
	}
}
