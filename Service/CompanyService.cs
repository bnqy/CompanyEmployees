﻿using Contracts;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
	internal sealed class CompanyService : ICompanyService
	{
		private readonly IRepositoryManager repositoryManager;
		private readonly ILoggerManager loggerManager;

		public CompanyService(IRepositoryManager repositoryManager, ILoggerManager loggerManager)
		{
			this.repositoryManager = repositoryManager;
			this.loggerManager = loggerManager;
		}
	}
}
