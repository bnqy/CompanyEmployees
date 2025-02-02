using Contracts;
using Entities.Models;
using Service.Contracts;
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

		public IEnumerable<Company> GetAllCompanies(bool trackChanges)
		{
			try
			{
				var companies = this.repositoryManager.Company.GetAllCompanies(trackChanges);

				return companies;
			}
			catch (Exception ex)
			{
				this.loggerManager.LogError($"Something went wrong in {nameof(GetAllCompanies)} method {ex}.");

				throw;
			}
		}
	}
}
