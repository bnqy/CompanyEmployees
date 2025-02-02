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

		public CompanyService(IRepositoryManager repositoryManager, ILoggerManager loggerManager)
		{
			this.repositoryManager = repositoryManager;
			this.loggerManager = loggerManager;
		}

		public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
		{
			try
			{
				var companies = this.repositoryManager.Company.GetAllCompanies(trackChanges);

				var companiesDto = companies.Select(c =>
				new CompanyDto(c.Id, c.Name ?? "", string.Join(' ', c.Address, c.Country)))
					.ToList();

				return companiesDto;
			}
			catch (Exception ex)
			{
				this.loggerManager.LogError($"Something went wrong in {nameof(GetAllCompanies)} method {ex}.");

				throw;
			}
		}
	}
}
