using EmployeeManager.Models.Data;

namespace EmployeeManager.Web.Models
{
    public class EmployeeIndexViewModel
    {
        public List<Employee> Employees { get; set; } = new();

        public string? SearchTerm { get; set; }

        public string? Department { get; set; }

        public bool? IsActive { get; set; }

        public List<string> Departments { get; set; } = new();
    }
}
