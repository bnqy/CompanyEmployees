using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
	public record CompanyForCreationDto 
	{
		[Required(ErrorMessage = "Name is required.")]
		[MaxLength(30, ErrorMessage = "MaxLength of Name is 30.")]
		public string? Name { get; set; }

		[Required(ErrorMessage = "Address is required.")]
		[MaxLength(30, ErrorMessage = "MaxLength of Address is 30.")]
		public string? Address { get; set; }

		[Required(ErrorMessage = "Country is required.")]
		[MaxLength(10, ErrorMessage = "MaxLength of Address is 10.")]
		public string? Country { get; set; }

		public IEnumerable<EmployeeForCreationDto> Employees { get; set; }
	};
}
