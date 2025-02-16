using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
	public class RepositoryContext : IdentityDbContext<User> // to integrate with identity.
	{
		public RepositoryContext(DbContextOptions options)
			: base(options)
		{
				
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder); // Migration to work properly.

			modelBuilder.ApplyConfiguration(new CompanyConfiguration()); // Apply initial data configs.
			modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
		}

		public DbSet<Company>? Companies { get; set; }

		public DbSet<Employee>? Employees { get; set; }
	}
}
