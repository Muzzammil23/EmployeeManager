using EmployeeManager.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManager.BL.Employees
{
    public interface IEmployeeLogic
    {
        Task<List<Employee>> GetAllEmployeesAsync();

        Task<Employee?> GetEmployeeByIdAsync(int id);

        Task<Employee> AddEmployeeAsync(Employee employee);

        Task<Employee?> UpdateEmployeeAsync(int id, Employee employee);

        Task<bool> DeleteEmployeeAsync(int id);

        Task<List<Employee>> SearchEmployeesAsync( string? searchTerm, string? department, bool? isActive);

        Task<List<string>> GetDepartmentsAsync();
    }
}
