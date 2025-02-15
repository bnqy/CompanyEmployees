using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Extensions.Utility
{
	public static class OrderQueryBuilder
	{
		public static string CreateOrderQuery<T>(string orderByQueryString)
		{
			var orderByStrings = orderByQueryString.Trim().Split(','); // Split into fields.

			var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance); // Gets the properties of T.

			var orderByBuilder = new StringBuilder();

			// Run through all params and check id they exist in the T.
			foreach (var param in orderByStrings)
			{
				if (string.IsNullOrWhiteSpace(param))
				{
					continue;
				}

				var propertyFromQueryName = param.Split(" ")[0];

				var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

				if (objectProperty is null)
				{
					continue;
				}

				var direction = param.EndsWith(" desc") ? "descending" : "ascending";

				orderByBuilder.Append($"{objectProperty.Name.ToString()} {direction}, ");
			}

			var orderQuery = orderByBuilder.ToString().TrimEnd(',', ' ');

			return orderQuery;
		}
	}
}
