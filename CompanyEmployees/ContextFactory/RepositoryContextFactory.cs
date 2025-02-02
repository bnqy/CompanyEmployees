using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repository;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CompanyEmployees.ContextFactory
{
	// Since our RepositoryContext class is in a Repository project
	// and not in the main one, this class will help our application
	// create a derived DbContext instance during the design time(DESIGN TIME).
	// This helps us find the RepositoryContext class in another project while executing migrations.
	// But we have the RepositoryManager service
	// registration, which happens at runtime, and during that registration, we
	// must have RepositoryContext registered as well in the runtime, so we
	// could inject it into other services(like RepositoryManager service). (see ServiceExtensions)
	public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
	{
		public RepositoryContext CreateDbContext(string[] args)
		{
			var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.Build();

			var builder = new DbContextOptionsBuilder<RepositoryContext>()
				.UseSqlServer(config.GetConnectionString("sqlConnection"),
				b => b.MigrationsAssembly("CompanyEmployees")); // Because migration assembly is not in MAIN project. It is in Repository project. And we changes it to MAIN project.

			return new RepositoryContext(builder.Options);
		}
	}
}
