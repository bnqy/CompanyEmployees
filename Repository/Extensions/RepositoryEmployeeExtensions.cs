using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace Repository.Extensions
{
	public static class RepositoryEmployeeExtensions
	{
		// FIltering by age.
		public static IQueryable<Employee> FilterEmployees(this IQueryable<Employee> employees,
			uint minAge, uint maxAge)
		{
			return employees.Where(e => (e.Age >= minAge && e.Age <= maxAge));
		}

		// Searching by term.
		public static IQueryable<Employee> Search(this IQueryable<Employee> employees,
			string searchTerm)
		{
			if (string.IsNullOrWhiteSpace(searchTerm))
			{
				return employees;
			}

			var lowerCaseTerm = searchTerm.Trim().ToLower();

			return employees.Where(e => e.Name.ToLower().Contains(lowerCaseTerm));
		}

		// Sorting def is name. EX = orderByQueryString: name,age desc
		public static IQueryable<Employee> Sort(this IQueryable<Employee> employees, string orderByQueryString)
		{
			if (string.IsNullOrWhiteSpace(orderByQueryString))
			{
				return employees.OrderBy(e => e.Name);
			}

			var orderByStrings = orderByQueryString.Trim().Split(','); // Split into fields.

			var propertyInfos = typeof(Employee).GetProperties(BindingFlags.Public | BindingFlags.Instance); // Gets the properties of Employee.
			
			var orderByBuilder = new StringBuilder();

			// Run through all params and check id they exist in the Employee.
			foreach (var param in orderByStrings)
			{
				if (string.IsNullOrWhiteSpace(param))
				{
					continue;
				}

				var propertyFromQueryName = param.Split(" ")[0];

				var objectProperty = propertyInfos.FirstOrDefault(pi =>
				pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

				if (objectProperty is null)
				{
					continue;
				}

				var direction = param.EndsWith(" desc") ? "descending" : "ascending";

				orderByBuilder.Append($"{objectProperty.Name.ToString()} {direction}, ");
			}

			var orderQuery = orderByBuilder.ToString().TrimEnd(',', ' ');

			if (orderQuery is null)
			{
				return employees.OrderBy(e => e.Name);
			}

			return employees.OrderBy(orderQuery);
		}
	}
}
