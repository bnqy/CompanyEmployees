using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Project for the database context and repository implementation.
namespace Repository
{
	public class RepositoryContext : DbContext
	{
		public RepositoryContext(DbContextOptions options)
			: base(options)
		{
				
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new CompanyConfiguration()); // Apply initial data configs.
			modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
		}

		public DbSet<Company>? Companies { get; set; }

		public DbSet<Employee>? Employees { get; set; }
	}
}
