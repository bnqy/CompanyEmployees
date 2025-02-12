using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
	public record EmployeeForCreationDto
	{
		[Required(ErrorMessage = "Name is required.")]
		[MaxLength(20, ErrorMessage = "MaxLength of Name is 20.")]
		public string? Name { get; init; }

		[Required(ErrorMessage = "Age is required.")]
		public int Age { get; init; }

		[Required(ErrorMessage = "Position is required.")]
		[MaxLength(20, ErrorMessage = "MaxLength of Position is 20.")]
		public string? Position { get; init; }
	};
}
