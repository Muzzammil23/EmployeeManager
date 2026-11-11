using System.ComponentModel.DataAnnotations;

namespace EmployeeManager.Models.Data
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Employee number is required.")]
        [StringLength(20, ErrorMessage = "Employee number cannot be longer than 20 characters")]
        public string EmployeeNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(100, ErrorMessage = "First name cannot be longer than 100 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(100, ErrorMessage = "Last name cannot be longer than 100 characters.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [StringLength(200, ErrorMessage = "Email cannot be longer than 200 characters.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Job title is required.")]
        [StringLength(100, ErrorMessage = "Job title cannot be longer than 100 characters.")]
        public string JobTitle { get; set; } = string.Empty;

        [Required(ErrorMessage = "Department is required.")]
        [StringLength(100, ErrorMessage = "Department cannot be longer than 100 characters.")]
        public string Department { get; set; } = string.Empty;

        [Range(0, 10000000, ErrorMessage = "Salary cannot be negative.")]
        public decimal Salary { get; set; }

        public DateTime DateHired { get; set; }

        public bool IsActive { get; set; }

        public byte[]? ProfileImageData { get; set; }

        public string? ProfileImageContentType { get; set; }
    }
}