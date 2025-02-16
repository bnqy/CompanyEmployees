﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
	public class CollectionByIdsBadRequestException : BadRequestException
	{
		public CollectionByIdsBadRequestException() 
			: base("Collection's and ids' count mismatch.")
		{
		}
	}
}
