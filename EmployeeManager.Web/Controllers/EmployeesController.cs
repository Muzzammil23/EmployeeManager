using EmployeeManager.BL.Employees;
using EmployeeManager.Models.Data;
using EmployeeManager.Web.Models; 
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManager.Web.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeLogic _employeeLogic;

        public EmployeesController(IEmployeeLogic employeeLogic)
        {
            _employeeLogic = employeeLogic;
        }

        public async Task<IActionResult> Index( string? searchTerm, string? department, bool? isActive)
        {
            var employees = await _employeeLogic
                    .SearchEmployeesAsync(
                        searchTerm,
                        department,
                        isActive);

            var departments = await _employeeLogic .GetDepartmentsAsync();

            var model = new EmployeeIndexViewModel
                {
                    Employees = employees,
                    SearchTerm = searchTerm,
                    Department = department,
                    IsActive = isActive,
                    Departments = departments
                };

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var employee = await _employeeLogic.GetEmployeeByIdAsync(id);
            if (employee is null)
                return NotFound();

            return View(employee);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateEmployeeViewModel
            {
                Employee = new Employee
                {
                    DateHired = DateTime.Today,
                    IsActive = true
                }
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
    CreateEmployeeViewModel model)
        {
            if (model.ProfileImageFile is not null &&
                model.ProfileImageFile.Length > 0)
            {
                var allowedContentTypes = new[]
                {
                    "image/jpeg",
                    "image/png",
                    "image/webp"
                };

                if (!allowedContentTypes.Contains(model.ProfileImageFile.ContentType))
                {
                    ModelState.AddModelError(
                        nameof(model.ProfileImageFile),
                        "Please upload a JPEG, PNG, or WebP image.");
                }

                if (model.ProfileImageFile.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError( nameof(model.ProfileImageFile), "The profile image must be 5 MB or smaller.");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.ProfileImageFile is not null && model.ProfileImageFile.Length > 0)
            {
                using var memoryStream = new MemoryStream();

                await model.ProfileImageFile.CopyToAsync(memoryStream);

                model.Employee.ProfileImageData = memoryStream.ToArray();

                model.Employee.ProfileImageContentType = model.ProfileImageFile.ContentType;
            }

            try
            {
                await _employeeLogic .AddEmployeeAsync(model.Employee);
            }
            catch (DuplicateEmployeeNumberException ex)
            {
                ModelState.AddModelError( "Employee.EmployeeNumber", ex.Message);

                return View(model);
            }

            return RedirectToAction( nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit( int id)
        {
            var employee = await _employeeLogic .GetEmployeeByIdAsync(id);

            if (employee is null)
            {
                return NotFound();
            }

            var model = new EditEmployeeViewModel
            {
                Employee = employee,

                HasExistingProfileImage =
                    employee.ProfileImageData is not null &&
                    employee.ProfileImageData.Length > 0 &&
                    !string.IsNullOrWhiteSpace(employee.ProfileImageContentType)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit( int id, EditEmployeeViewModel model)
        {
            var existingEmployee = await _employeeLogic .GetEmployeeByIdAsync(id);

            if (existingEmployee is null)
            {
                return NotFound();
            }

            if (model.NewProfileImageFile is not null &&
                model.NewProfileImageFile.Length > 0)
            {
                var allowedContentTypes = new[]
                {
                    "image/jpeg",
                    "image/png",
                    "image/webp"
                };

                if (!allowedContentTypes.Contains(
                        model.NewProfileImageFile.ContentType))
                {
                    ModelState.AddModelError(
                        nameof(model.NewProfileImageFile),
                        "Please upload a JPEG, PNG, or WebP image.");
                }

                if (model.NewProfileImageFile.Length >
                    5 * 1024 * 1024)
                {
                    ModelState.AddModelError(
                        nameof(model.NewProfileImageFile),
                        "The profile image must be 5 MB or smaller.");
                }
            }

            if (model.RemoveProfileImage &&
                model.NewProfileImageFile is not null &&
                model.NewProfileImageFile.Length > 0)
            {
                ModelState.AddModelError( nameof(model.NewProfileImageFile), "Choose either a new profile image or remove the current image, not both.");
            }

            if (!ModelState.IsValid)
            {
                model.Employee.Id = id;

                model.HasExistingProfileImage =
                    existingEmployee.ProfileImageData is not null &&
                    existingEmployee.ProfileImageData.Length > 0 &&
                    !string.IsNullOrWhiteSpace(
                        existingEmployee.ProfileImageContentType);

                return View(model);
            }

            existingEmployee.EmployeeNumber = model.Employee.EmployeeNumber;

            existingEmployee.FirstName = model.Employee.FirstName;

            existingEmployee.LastName = model.Employee.LastName;

            existingEmployee.Email = model.Employee.Email;

            existingEmployee.JobTitle = model.Employee.JobTitle;

            existingEmployee.Department = model.Employee.Department;

            existingEmployee.Salary = model.Employee.Salary;

            existingEmployee.DateHired = model.Employee.DateHired;

            existingEmployee.IsActive = model.Employee.IsActive;

            if (model.RemoveProfileImage)
            {
                existingEmployee.ProfileImageData = null;

                existingEmployee.ProfileImageContentType = null;
            }
            else if ( model.NewProfileImageFile is not null && model.NewProfileImageFile.Length > 0)
            {
                using var memoryStream = new MemoryStream();

                await model.NewProfileImageFile .CopyToAsync(memoryStream);

                existingEmployee.ProfileImageData = memoryStream.ToArray();

                existingEmployee.ProfileImageContentType = model.NewProfileImageFile.ContentType;
            }

            try
            {
                var updatedEmployee = await _employeeLogic .UpdateEmployeeAsync( id, existingEmployee);

                if (updatedEmployee is null)
                {
                    return NotFound();
                }
            }
            catch (DuplicateEmployeeNumberException ex)
            {
                model.Employee.Id = id;

                model.HasExistingProfileImage =
                    existingEmployee.ProfileImageData is not null &&
                    existingEmployee.ProfileImageData.Length > 0 &&
                    !string.IsNullOrWhiteSpace(
                        existingEmployee.ProfileImageContentType);

                ModelState.AddModelError(
                    "Employee.EmployeeNumber",
                    ex.Message);

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeLogic .GetEmployeeByIdAsync(id);

            if (employee is null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _employeeLogic .DeleteEmployeeAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ProfileImage( int id)
        {
            var employee = await _employeeLogic.GetEmployeeByIdAsync(id);

            if (employee is null ||
                employee.ProfileImageData is null ||
                employee.ProfileImageData.Length == 0 ||
                string.IsNullOrWhiteSpace(employee.ProfileImageContentType))
            {
                return NotFound();
            }

            return File(employee.ProfileImageData, employee.ProfileImageContentType);
        }
    }
}