using EmployeeManager.Models.Data;

namespace EmployeeManager.Web.Models
{
    public class EditEmployeeViewModel
    {
        public Employee Employee { get; set; } = new();

        public IFormFile? NewProfileImageFile { get; set; }

        public bool RemoveProfileImage { get; set; }

        public bool HasExistingProfileImage { get; set; }
    }
}
