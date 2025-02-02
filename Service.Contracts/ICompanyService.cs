﻿using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//  Service.Contracts will hold service interfaces which encapsulates main business logic
namespace Service.Contracts
{
	public interface ICompanyService
	{
		IEnumerable<Company> GetAllCompanies(bool trackChanges);
	}
}
