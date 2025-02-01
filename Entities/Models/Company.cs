using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// EF Core uses thede models(Entities) to map out DB
namespace Entities.Models
{
	public class Company
	{
		[Column("CompanyId")]
		public Guid Id { get; set; }

		[Required(ErrorMessage = "Name is required.")]
		[MaxLength(60, ErrorMessage = "Max length of Name is 60.")]
		public string? Name { get; set; }

		[Required(ErrorMessage = "Address is required.")]
		[MaxLength(60, ErrorMessage = "Max length of Address is 60.")]
		public string? Address { get; set; }

		public string? Country { get; set; }

		public ICollection<Employee>? Employees { get; set; } // navigational property: defines relationship between entities
	}
}
