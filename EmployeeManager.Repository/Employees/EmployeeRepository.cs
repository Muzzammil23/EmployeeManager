using EmployeeManager.Data.Context;
using EmployeeManager.Models.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManager.Repository.Employees
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee?> UpdateEmployeeAsync(int id, Employee employee)
        {
            var existingEmployee = await _context.Employees.FindAsync(id);
            if (existingEmployee is null)
                return null;

            existingEmployee.EmployeeNumber = employee.EmployeeNumber;

            existingEmployee.FirstName = employee.FirstName;

            existingEmployee.LastName = employee.LastName;

            existingEmployee.Email = employee.Email;

            existingEmployee.JobTitle = employee.JobTitle;

            existingEmployee.Department = employee.Department;

            existingEmployee.Salary = employee.Salary;

            existingEmployee.DateHired = employee.DateHired;

            existingEmployee.IsActive = employee.IsActive;

            existingEmployee.ProfileImageData = employee.ProfileImageData;

            existingEmployee.ProfileImageContentType = employee.ProfileImageContentType;

            await _context.SaveChangesAsync();
            return existingEmployee;
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var existingEmployee = await _context.Employees.FindAsync(id);
            if (existingEmployee is null)
                return false;

            _context.Employees.Remove(existingEmployee);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EmployeeNumberExistsAsync( string employeeNumber, int? excludeEmployeeId = null)
        {
            return await _context.Employees
                .AnyAsync(employee => 
                    employee.EmployeeNumber == employeeNumber &&
                    (!excludeEmployeeId.HasValue || employee.Id != excludeEmployeeId.Value));
        }

        public async Task<List<Employee>> SearchEmployeesAsync(string? searchTerm, string? department, bool? isActive) 
        {
            var query = _context.Employees.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(employee=>
                employee.EmployeeNumber.Contains(searchTerm) ||
                employee.FirstName.Contains(searchTerm) ||
                employee.LastName.Contains(searchTerm) ||
                employee.Email.Contains(searchTerm) 
                );
            }

            if (!string.IsNullOrWhiteSpace(department))
            {
                query = query.Where(employee => employee.Department == department);
            }

            if (isActive.HasValue)
            {
                query = query.Where(employee => employee.IsActive == isActive.Value);
            }

            return await query
            .OrderBy(employee => employee.LastName)
            .ThenBy(employee => employee.FirstName)
            .ToListAsync();
        }

        public async Task<List<string>> GetDepartmentsAsync()
        {
            return await _context.Employees
                .AsNoTracking()
                .Where(employee =>
                    employee.Department != "")
                .Select(employee =>
                    employee.Department)
                .Distinct()
                .OrderBy(department =>
                    department)
                .ToListAsync();
        }

    }
}