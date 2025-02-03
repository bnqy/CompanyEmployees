using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
	// Abstract. Because it is a base class for all individual NotFound classes. 
	public abstract class NotFoundException : Exception
	{
		protected NotFoundException(string message) 
			: base(message)
		{ }
	}
}
