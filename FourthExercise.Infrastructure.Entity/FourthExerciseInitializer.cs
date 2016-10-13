using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

using FourthExercise.Infrastructure.Entity.Models;

namespace FourthExercise.Infrastructure.Entity
{
    public static class FourthExerciseSeed
    {
        public enum JobRoleId
        {
            Employee = 1,
            Manager = 2,
            ManagersManager = 3
        }

        public static readonly List<JobRole> JobRoles = new List<JobRole>()
        {
            new JobRole() { Id = (int)JobRoleId.Employee, Name = "Employee" },
            new JobRole() { Id = (int)JobRoleId.Manager, Name = "Manager" },
            new JobRole() { Id = (int)JobRoleId.ManagersManager, Name = "Manager's Manager" }
        };

        public static readonly List<Employee> Employees = new List<Employee>()
        {
            new Employee() { Id = 1, FirstName = "Petar", LastName = "Minkov", Email = "m_minkov@gmail.com", JobRoleId = 1, Salary = 3500 },
            new Employee() { Id = 2, FirstName = "Ivan", LastName = "Petrov", Email = "i_petrov@gmail.com", JobRoleId = 1, Salary = 3500 },
            new Employee() { Id = 3, FirstName = "Petar", LastName = "Ivanov", Email = "p_ivanov@gmail.com", JobRoleId = 2, Salary = 4000 },
            new Employee() { Id = 4, FirstName = "Dimitar", LastName = "Kirilov", Email = "d_kirilov@gmail.com", JobRoleId = 3, Salary = 4500 }
        };
    }

    // DropCreateDatabaseAlways<FourthExerciseContext>
    // DropCreateDatabaseIfModelChanges<FourthExerciseContext>
    public class FourthExerciseInitializer : DropCreateDatabaseAlways<FourthExerciseContext>
    {
        protected override void Seed(FourthExerciseContext context)
        {
            context.JobRoles.AddRange(FourthExerciseSeed.JobRoles);
            context.Employees.AddRange(FourthExerciseSeed.Employees);

            base.Seed(context);
        }
    }
}