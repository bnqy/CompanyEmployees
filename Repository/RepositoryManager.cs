using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
	public class RepositoryManager : IRepositoryManager
	{
		private readonly RepositoryContext repositoryContext;

		private readonly Lazy<ICompanyRepository> companyRepository;

		private readonly Lazy<IEmployeeRepository> employeeRepository;

		public RepositoryManager(RepositoryContext repositoryContext)
		{
			this.repositoryContext = repositoryContext;
			this.companyRepository = new Lazy<ICompanyRepository>(() => new CompanyRepository(repositoryContext));
			this.employeeRepository = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(repositoryContext));
		}

		public ICompanyRepository Company => this.companyRepository.Value;

		public IEmployeeRepository Employee => this.employeeRepository.Value;

		public void Save()
		{
			this.repositoryContext.SaveChanges();
		}
	}
}
