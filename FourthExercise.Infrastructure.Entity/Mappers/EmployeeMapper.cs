using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;

using FourthExercise.Models;
using FourthExercise.Infrastructure.Repositories;
using FourthExercise.Infrastructure.Entity.Models;

namespace FourthExercise.Infrastructure.Entity.Mappers
{
    internal static class EmployeeMapper
    {
        public static EmployeeModel MapEmployeeToModel(Employee employee)
        {
            if (employee == null) { return null; }

            EmployeeModel employeeModel = new EmployeeModel();

            employeeModel.Id = employee.Id;
            employeeModel.FirstName = employee.FirstName;
            employeeModel.LastName = employee.LastName;
            employeeModel.Email = employee.Email;
            employeeModel.JobRoleId = employee.JobRoleId;
            employeeModel.JobRole = JobRoleMapper.MapJobRoleToModel(employee.JobRole);
            employeeModel.Salary = employee.Salary;

            return employeeModel;
        }

        public static Employee MapModelToEmployee(EmployeeModel employeeModel)
        {
            if (employeeModel == null) { return null; }

            Employee employee = new Employee();

            employee.Id = employeeModel.Id;
            employee.FirstName = employeeModel.FirstName;
            employee.LastName = employeeModel.LastName;
            employee.Email = employeeModel.Email;
            employee.JobRoleId = employeeModel.JobRoleId;
            employee.JobRole = JobRoleMapper.MapModelToJobRole(employeeModel.JobRole);
            employee.Salary = employeeModel.Salary;

            return employee;
        }
    }
}
