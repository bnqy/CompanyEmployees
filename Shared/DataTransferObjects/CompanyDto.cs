using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// We are not using the validation attributes. 
// Because we are gonna use this for data transfer only.
namespace Shared.DataTransferObjects
{
	// [Serializable] // --> _x003C_FullAddress_x003E_k__BackingField
	public record CompanyDto 
	{
		public Guid Id { get; init; }
		public string? Name { get; init; }
		public string? FullAddress { get; set; }
	};
}
