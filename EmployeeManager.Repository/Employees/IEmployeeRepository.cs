using EmployeeManager.Models.Data;

namespace EmployeeManager.Repository.Employees
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>>GetAllEmployeesAsync();
        Task<Employee?>GetEmployeeByIdAsync(int id);
        Task<Employee>AddEmployeeAsync(Employee employee);
        Task<Employee?>UpdateEmployeeAsync(int id, Employee employee);
        Task<bool>DeleteEmployeeAsync(int id);
        Task<bool> EmployeeNumberExistsAsync(string employeeNumber, int? excludeEmployeeId = null);
        Task<List<Employee>> SearchEmployeesAsync(string? searchTerm, string? department, bool? isActive);
        Task<List<string>> GetDepartmentsAsync();
    }
}