using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//  Service.Contracts will hold service interfaces which encapsulates main business logic
namespace Service.Contracts
{
	public interface ICompanyService
	{
		IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges);
		CompanyDto GetCompany(Guid companyId, bool trackChanges);
		CompanyDto CreateCompany(CompanyForCreationDto company);
		IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges);
		(IEnumerable<CompanyDto> companies, string ids) CreateCompanyCollection (IEnumerable<CompanyForCreationDto> companyCollection);
		void DeleteCompany(Guid id, bool trackChanges);
		void UpdateCompany(Guid companyId, CompanyForUpdateDto companyForUpdateDto, bool trackChanges);
	}
}
