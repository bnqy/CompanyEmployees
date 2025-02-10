using AutoMapper;
using Contracts;
using Entities.Exceptions;
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

		public CompanyDto CreateCompany(CompanyForCreationDto company)
		{
			var companyEntity = this.mapper.Map<Company>(company);
			this.repositoryManager.Company.CreateCompany(companyEntity);
			this.repositoryManager.Save();

			var companyDto = this.mapper.Map<CompanyDto>(companyEntity);

			return companyDto;
		}

		public (IEnumerable<CompanyDto> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection)
		{
			if (companyCollection is null)
			{
				throw new CompanyCollectionBadRequest();
			}

			var companies = this.mapper.Map<IEnumerable<Company>>(companyCollection);

			foreach (var company in companies)
			{
				this.repositoryManager.Company.CreateCompany(company);
			}

			this.repositoryManager.Save();

			var companyCollectionDto = this.mapper.Map<IEnumerable<CompanyDto>>(companies);

			var ids = string.Join(",", companyCollectionDto.Select(x => x.Id));

			return (companyCollectionDto, ids);
		}

		public void DeleteCompany(Guid id, bool trackChanges)
		{
			var company = this.repositoryManager.Company.GetCompany(id, trackChanges);

			if (company is null)
			{
				throw new CompanyNotFoundException(id);
			}

			this.repositoryManager.Company.DeleteCompany(company);

			this.repositoryManager.Save();
		}

		public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
		{
			var companies = this.repositoryManager.Company.GetAllCompanies(trackChanges);

			var companiesDto = this.mapper.Map<IEnumerable<CompanyDto>>(companies);

			return companiesDto;
		}

		public IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
		{
			if (ids is null)
			{
				throw new IdParametersBadRequestException();
			}

			var companies = this.repositoryManager.Company.GetByIds(ids, trackChanges);

			if (companies.Count() != ids.Count())
			{
				throw new CollectionByIdsBadRequestException();
			}

			var companiesDto = this.mapper.Map<IEnumerable<CompanyDto>>(companies);

			return companiesDto;
		}

		public CompanyDto GetCompany(Guid companyId, bool trackChanges)
		{
			var company = this.repositoryManager.Company.GetCompany(companyId, trackChanges);

			if (company is null)
			{
				throw new CompanyNotFoundException(companyId);
			}

			var companyDto = this.mapper.Map<CompanyDto>(company);

			return companyDto;
		}

		public void UpdateCompany(Guid companyId, CompanyForUpdateDto companyForUpdateDto, bool trackChanges)
		{
			var company = this.repositoryManager.Company.GetCompany(companyId, trackChanges);

			if (company is null)
			{
				throw new CompanyNotFoundException(companyId);
			}

			this.mapper.Map(companyForUpdateDto, company);

			this.repositoryManager.Save();
		}
	}
}
