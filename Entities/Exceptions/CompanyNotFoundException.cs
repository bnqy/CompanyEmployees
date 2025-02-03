﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
	// Child of NotFoundException.
	public sealed class CompanyNotFoundException : NotFoundException
	{
		public CompanyNotFoundException(Guid companyId) 
			: base($"The company with id: {companyId} does not exist in the DB.")
		{
		}
	}
}
