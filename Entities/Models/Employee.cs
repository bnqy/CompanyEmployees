using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
	public class Employee
	{
		[Column("EmployeeId")]
		public Guid Id { get; set; }

		[Required(ErrorMessage = "The name of employee is required.")]
		[MaxLength(30, ErrorMessage = "The max length of name is 60.")]
		public string? Name { get; set; }

		[Required(ErrorMessage = "The employee's age is required.")]
		[Range(1, 200)]
		public int Age { get; set; }

		[Required(ErrorMessage = "The employee's position is required.")]
		[MaxLength(20, ErrorMessage = "The max length of position is 20.")]
		public string? Position { get; set; }

		[ForeignKey(nameof(Company))]
		public Guid CompanyId { get; set; }

		public Company? Company { get; set; } // navigational property: defines relationship between entities
	}
}
