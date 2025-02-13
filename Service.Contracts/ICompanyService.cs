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
		Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges);
		Task<CompanyDto> GetCompanyAsync(Guid companyId, bool trackChanges);
		Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company);
		Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
		Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync (IEnumerable<CompanyForCreationDto> companyCollection);
		Task DeleteCompanyAsync(Guid id, bool trackChanges);
		Task UpdateCompanyAsync(Guid companyId, CompanyForUpdateDto companyForUpdateDto, bool trackChanges);
	}
}
