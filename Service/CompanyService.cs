using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

		public async Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company)
		{
			var companyEntity = this.mapper.Map<Company>(company);
			this.repositoryManager.Company.CreateCompany(companyEntity);
			await this.repositoryManager.SaveAsync();

			var companyDto = this.mapper.Map<CompanyDto>(companyEntity);

			return companyDto;
		}

		public async Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companyCollection)
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

			await this.repositoryManager.SaveAsync();

			var companyCollectionDto = this.mapper.Map<IEnumerable<CompanyDto>>(companies);

			var ids = string.Join(",", companyCollectionDto.Select(x => x.Id));

			return (companyCollectionDto, ids);
		}

		public async Task DeleteCompanyAsync(Guid id, bool trackChanges)
		{
			var company = await this.GetCompanyAndCheckIfItExists(id, trackChanges);

			this.repositoryManager.Company.DeleteCompany(company);

			await this.repositoryManager.SaveAsync();
		}

		public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges)
		{
			var companies = await this.repositoryManager.Company.GetAllCompaniesAsync(trackChanges);

			var companiesDto = this.mapper.Map<IEnumerable<CompanyDto>>(companies);

			return companiesDto;
		}

		public async Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
		{
			if (ids is null)
			{
				throw new IdParametersBadRequestException();
			}

			var companies = await this.repositoryManager.Company.GetByIdsAsync(ids, trackChanges);

			if (companies.Count() != ids.Count())
			{
				throw new CollectionByIdsBadRequestException();
			}

			var companiesDto = this.mapper.Map<IEnumerable<CompanyDto>>(companies);

			return companiesDto;
		}

		public async Task<CompanyDto> GetCompanyAsync(Guid companyId, bool trackChanges)
		{
			var company = await this.GetCompanyAndCheckIfItExists(companyId, trackChanges);

			var companyDto = this.mapper.Map<CompanyDto>(company);

			return companyDto;
		}

		public async Task UpdateCompanyAsync(Guid companyId, CompanyForUpdateDto companyForUpdateDto, bool trackChanges)
		{
			var company = await this.GetCompanyAndCheckIfItExists(companyId, trackChanges);

			this.mapper.Map(companyForUpdateDto, company);

			await this.repositoryManager.SaveAsync();
		}

		private async Task<Company> GetCompanyAndCheckIfItExists(Guid id, bool trackChanges)
		{
			var company = await this.repositoryManager.Company.GetCompanyAsync(id, trackChanges);

			if (company is null)
			{
				throw new CompanyNotFoundException(id);
			}

			return company;
		}
	}
}
