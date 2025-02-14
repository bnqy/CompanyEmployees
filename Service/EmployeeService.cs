using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
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

		public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreationDto, bool trackChanges)
		{
			/*var company = await this.repositoryManager.Company.GetCompanyAsync(companyId, trackChanges);

			if (company is null)
			{
				throw new CompanyNotFoundException(companyId);
			}*/

			await this.CheckIfCompanyExists(companyId, trackChanges);

			var employee = this.mapper.Map<Employee>(employeeForCreationDto);
			this.repositoryManager.Employee.CreateEmployeeForCompany(companyId, employee);
			
			await this.repositoryManager.SaveAsync();

			var employeeDto = this.mapper.Map<EmployeeDto>(employee);

			return employeeDto;
		}

		public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges)
		{
			/*var company = await this.repositoryManager.Company.GetCompanyAsync(companyId, trackChanges);

			if (company is null)
			{
				throw new CompanyNotFoundException(companyId);
			}

			var employee = await this.repositoryManager.Employee.GetEmployeeAsync(companyId, id, trackChanges);

			if (employee is null)
			{
				throw new EmployeeNotFoundException(id);
			}*/

			await this.CheckIfCompanyExists(companyId, trackChanges);
			var employee = await this.GetEmployeeForCompanyAndCheckIfItExists(companyId, id, trackChanges);

			this.repositoryManager.Employee.DeleteEmployee(employee);
			await this.repositoryManager.SaveAsync();
		}

		public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
		{
			/*var company = await this.repositoryManager.Company.GetCompanyAsync(companyId, trackChanges);

			if (company is null)
			{
				throw new CompanyNotFoundException(companyId);
			}

			var employee = await this.repositoryManager.Employee.GetEmployeeAsync(companyId, id, trackChanges);

			if (employee is null)
			{
				throw new EmployeeNotFoundException(id);
			}*/

			await CheckIfCompanyExists(companyId, trackChanges);

			var employee = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, trackChanges);

			var employeeDto = this.mapper.Map<EmployeeDto>(employee);

			return employeeDto;
		}

		public async Task<(EmployeeForUpdateDto employeeForPatch, Employee employee)> GetEmployeeForPatchAsync(
			Guid companyId, Guid id, 
			//EmployeeForUpdateDto employeeForUpdateDto, 
			bool compTrackChanges, bool empTrackChanges)
		{
			/*var company = await this.repositoryManager.Company.GetCompanyAsync(companyId, compTrackChanges);

			if (company is null)
			{
				throw new CompanyNotFoundException(companyId);
			}

			var employee = await this.repositoryManager.Employee.GetEmployeeAsync(companyId, id, empTrackChanges);

			if (employee is null)
			{
				throw new EmployeeNotFoundException(id);
			}*/

			await this.CheckIfCompanyExists(companyId, compTrackChanges);

			var employee = await this.GetEmployeeForCompanyAndCheckIfItExists(companyId, id, empTrackChanges);

			var employeeForPatch = this.mapper.Map<EmployeeForUpdateDto>(employee);

			return (employeeForPatch, employee);
		}

		public async Task<(IEnumerable<EmployeeDto> employees, MetaData metaData)> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
		{
			await this.CheckIfCompanyExists(companyId, trackChanges);

			var employeesWithMetaData = await this.repositoryManager.Employee.GetEmployeesAsync(companyId, employeeParameters, trackChanges);

			var employeesDto = this.mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetaData);

			return (employees: employeesDto, metaData: employeesWithMetaData.MetaData);
		}

		public async Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeForPatch, Employee employee)
		{
			this.mapper.Map(employeeForPatch, employee);

			await this.repositoryManager.SaveAsync();
		}

		public async Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdateDto, bool compTrackChanges, bool empTrackChanges)
		{
			/*var company = await this.repositoryManager.Company.GetCompanyAsync(companyId, compTrackChanges);

			if (company is null)
			{
				throw new CompanyNotFoundException(companyId);
			}

			var employee = await this.repositoryManager.Employee.GetEmployeeAsync(companyId, id, empTrackChanges);

			if (employee is null)
			{
				throw new EmployeeNotFoundException(id);
			}*/

			await this.CheckIfCompanyExists(companyId, compTrackChanges);
			var employee = await this.GetEmployeeForCompanyAndCheckIfItExists(companyId, id, empTrackChanges);

			this.mapper.Map(employeeForUpdateDto, employee);

			await this.repositoryManager.SaveAsync();
		}

		private async Task CheckIfCompanyExists(Guid companyId, bool trackChanges)
		{
			var company = await this.repositoryManager.Company.GetCompanyAsync(companyId, trackChanges);

			if (company is null)
			{
				throw new CompanyNotFoundException(companyId);
			}
		}

		private async Task<Employee> GetEmployeeForCompanyAndCheckIfItExists(Guid companyId, Guid id, bool trackChanges)
		{
			var employee = await this.repositoryManager.Employee.GetEmployeeAsync(companyId, id, trackChanges);

			if (employee is null)
			{
				throw new EmployeeNotFoundException(id);
			}

			return employee;
		}
	}
}
