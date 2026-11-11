using EmployeeManager.Models.Data;
using EmployeeManager.Repository.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManager.BL.Employees
{
    public class EmployeeLogic : IEmployeeLogic
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeLogic(IEmployeeRepository employeeRepository) 
        {
            _employeeRepository=employeeRepository;
        }

        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            var employeeNumberExists = await _employeeRepository.EmployeeNumberExistsAsync(employee.EmployeeNumber);
            if (employeeNumberExists)
                throw new DuplicateEmployeeNumberException(employee.EmployeeNumber);

            return await _employeeRepository.AddEmployeeAsync(employee);
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            return await _employeeRepository.DeleteEmployeeAsync(id);
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _employeeRepository.GetAllEmployeesAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _employeeRepository.GetEmployeeByIdAsync(id);
        }

        public async Task<Employee?> UpdateEmployeeAsync(int id, Employee employee)
        {
            var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (existingEmployee is null)
                return null;

            var employeeNumberExists = await _employeeRepository.EmployeeNumberExistsAsync(employee.EmployeeNumber, id);

            if (employeeNumberExists)
                throw new DuplicateEmployeeNumberException($"{employee.EmployeeNumber}");

            return await _employeeRepository.UpdateEmployeeAsync(id, employee);
        }

        public async Task<List<Employee>> SearchEmployeesAsync( string? searchTerm, string? department, bool? isActive)
        {
            return await _employeeRepository
                .SearchEmployeesAsync(
                    searchTerm,
                    department,
                    isActive);
        }

        public async Task<List<string>> GetDepartmentsAsync()
        {
            return await _employeeRepository
                .GetDepartmentsAsync();
        }
    }
}
