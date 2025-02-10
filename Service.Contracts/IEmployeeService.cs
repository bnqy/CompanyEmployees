﻿using Entities.Models;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
	public interface IEmployeeService
	{
		IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges);
		EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges);
		EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreationDto, bool trackChanges);
		void DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges);
		void UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdateDto, bool compTrackChanges, bool empTrackChanges);
		(EmployeeForUpdateDto employeeForPatch, Employee employee) GetEmployeeForPatch(Guid companyId, Guid id, /*EmployeeForUpdateDto employeeForUpdateDto,*/ bool compTrackChanges, bool empTrackChanges);
		void SaveChangesForPatch(EmployeeForUpdateDto employeeForPatch, Employee employee);
	}
}
