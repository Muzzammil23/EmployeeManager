using EmployeeManager.Models.Data;

namespace EmployeeManager.Web.Models
{
    public class CreateEmployeeViewModel
    {
        public Employee Employee { get; set; } = new();

        public IFormFile? ProfileImageFile { get; set; }
    }
}
