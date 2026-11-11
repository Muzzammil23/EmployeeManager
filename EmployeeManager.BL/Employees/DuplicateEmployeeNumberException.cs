using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManager.BL.Employees
{
    public class DuplicateEmployeeNumberException : Exception
    {
        public DuplicateEmployeeNumberException(string employeeNumber) : base($"Employee number '{employeeNumber}' already exists.")
        {
        }
    }
}
