using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
	public class MaxAgeRangeBadRequestException : BadRequestException
	{
		public MaxAgeRangeBadRequestException() : base("Max age is less than a min age.")
		{
		}
	}
}
