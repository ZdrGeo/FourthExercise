using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

using FourthExercise.Models;

namespace FourthExercise.Infrastructure.Entity
{
    // DropCreateDatabaseAlways<FourthExerciseContext>
    // DropCreateDatabaseIfModelChanges<FourthExerciseContext>
    public class FourthExerciseInitializer : DropCreateDatabaseAlways<FourthExerciseContext>
    {
        protected override void Seed(FourthExerciseContext context)
        {
            List<JobRole> jobRoles = new List<JobRole>();

            jobRoles.Add(new JobRole() { Id = 1, Name = "Employee" });
            jobRoles.Add(new JobRole() { Id = 2, Name = "Manager" });
            jobRoles.Add(new JobRole() { Id = 3, Name = "Manager's Manager" });

            context.JobRoles.AddRange(jobRoles);

            List<Employee> employees = new List<Employee>();

            employees.Add(new Employee() { Id = 1, FirstName = "Petar", LastName = "Minkov", Email = "m_minkov@gmail.com", JobRoleId = 1, Salary = 3500 });
            employees.Add(new Employee() { Id = 2, FirstName = "Ivan", LastName = "Petrov", Email = "i_petrov@gmail.com", JobRoleId = 1, Salary = 3500 });
            employees.Add(new Employee() { Id = 3, FirstName = "Petar", LastName = "Ivanov", Email = "p_ivanov@gmail.com", JobRoleId = 2, Salary = 4000 });
            employees.Add(new Employee() { Id = 4, FirstName = "Dimitar", LastName = "Kirilov", Email = "d_kirilov@gmail.com", JobRoleId = 3, Salary = 4500 });

            context.Employees.AddRange(employees);

            base.Seed(context);
        }
    }
}