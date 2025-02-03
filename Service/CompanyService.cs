using AutoMapper;
using Contracts;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
	internal sealed class CompanyService : ICompanyService
	{
		private readonly IRepositoryManager repositoryManager;
		private readonly ILoggerManager loggerManager;
		private readonly IMapper mapper;

		public CompanyService(IRepositoryManager repositoryManager,
			ILoggerManager loggerManager,
			IMapper mapper)
		{
			this.repositoryManager = repositoryManager;
			this.loggerManager = loggerManager;
			this.mapper = mapper;
		}

		public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
		{
			var companies = this.repositoryManager.Company.GetAllCompanies(trackChanges);

			var companiesDto = this.mapper.Map<IEnumerable<CompanyDto>>(companies);

			return companiesDto;
		}

		public CompanyDto GetCompany(Guid companyId, bool trackChanges)
		{
			var company = this.repositoryManager.Company.GetCompany(companyId, trackChanges);

			// check for null

			var companyDto = this.mapper.Map<CompanyDto>(company);

			return companyDto;
		}
	}
}
