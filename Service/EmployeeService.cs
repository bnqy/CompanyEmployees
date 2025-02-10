using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
	internal sealed class EmployeeService : IEmployeeService
	{
		private readonly IRepositoryManager repositoryManager;
		private readonly ILoggerManager loggerManager;
		private readonly IMapper mapper;

		public EmployeeService(IRepositoryManager repositoryManager,
			ILoggerManager loggerManager,
			IMapper mapper)
		{
			this.repositoryManager = repositoryManager;
			this.loggerManager = loggerManager;
			this.mapper = mapper;
		}

		public EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreationDto, bool trackChanges)
		{
			var company = this.repositoryManager.Company.GetCompany(companyId, trackChanges);

			if (company is null)
			{
				throw new CompanyNotFoundException(companyId);
			}

			var employee = this.mapper.Map<Employee>(employeeForCreationDto);
			this.repositoryManager.Employee.CreateEmployeeForCompany(companyId, employee);
			this.repositoryManager.Save();

			var employeeDto = this.mapper.Map<EmployeeDto>(employee);

			return employeeDto;
		}

		public void DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges)
		{
			var company = this.repositoryManager.Company.GetCompany(companyId, trackChanges);

			if (company is null)
			{
				throw new CompanyNotFoundException(companyId);
			}

			var employee = this.repositoryManager.Employee.GetEmployee(companyId, id, trackChanges);

			if (employee is null)
			{
				throw new EmployeeNotFoundException(id);
			}

			this.repositoryManager.Employee.DeleteEmployee(employee);
			this.repositoryManager.Save();
		}

		public EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges)
		{
			var company = this.repositoryManager.Company.GetCompany(companyId, trackChanges);

			if (company is null)
			{
				throw new CompanyNotFoundException(companyId);
			}

			var employee = this.repositoryManager.Employee.GetEmployee(companyId, id, trackChanges);

			if (employee is null)
			{
				throw new EmployeeNotFoundException(id);
			}

			var employeeDto = this.mapper.Map<EmployeeDto>(employee);

			return employeeDto;
		}

		public (EmployeeForUpdateDto employeeForPatch, Employee employee) GetEmployeeForPatch(
			Guid companyId, Guid id, 
			//EmployeeForUpdateDto employeeForUpdateDto, 
			bool compTrackChanges, bool empTrackChanges)
		{
			var company = this.repositoryManager.Company.GetCompany(companyId, compTrackChanges);

			if (company is null)
			{
				throw new CompanyNotFoundException(companyId);
			}

			var employee = this.repositoryManager.Employee.GetEmployee(companyId, id, empTrackChanges);

			if (employee is null)
			{
				throw new EmployeeNotFoundException(id);
			}

			var employeeForPatch = this.mapper.Map<EmployeeForUpdateDto>(employee);

			return (employeeForPatch, employee);
		}

		public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
		{
			var company = this.repositoryManager.Company.GetCompany(companyId, trackChanges);

			if (company is null)
			{
				throw new CompanyNotFoundException(companyId);
			}

			var employees = this.repositoryManager.Employee.GetEmployees(companyId, trackChanges);

			var employeesDto = this.mapper.Map<IEnumerable<EmployeeDto>>(employees);

			return employeesDto;
		}

		public void SaveChangesForPatch(EmployeeForUpdateDto employeeForPatch, Employee employee)
		{
			this.mapper.Map(employeeForPatch, employee);

			this.repositoryManager.Save();
		}

		public void UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdateDto, bool compTrackChanges, bool empTrackChanges)
		{
			var company = this.repositoryManager.Company.GetCompany(companyId, compTrackChanges);

			if (company is null)
			{
				throw new CompanyNotFoundException(companyId);
			}

			var employee = this.repositoryManager.Employee.GetEmployee(companyId, id, empTrackChanges);

			if (employee is null)
			{
				throw new EmployeeNotFoundException(id);
			}

			this.mapper.Map(employeeForUpdateDto, employee);

			this.repositoryManager.Save();
		}
	}
}
