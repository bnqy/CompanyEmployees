using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// We are not using the validation attributes. 
// Because we are gonna use this for data transfer only.
namespace Shared.DataTransferObjects
{
	public record CompanyDto(Guid Id, string Name, string FullAddress);
}
