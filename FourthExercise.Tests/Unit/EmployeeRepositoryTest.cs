using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using FourthExercise.Models;
using FourthExercise.Infrastructure;
using FourthExercise.Infrastructure.Repositories;
using FourthExercise.Infrastructure.Entity;
using FourthExercise.Infrastructure.Entity.Models;
using FourthExercise.Infrastructure.Entity.Mappers;
using FourthExercise.Infrastructure.Entity.Repositories;
using FourthExercise.Tests.Msdn;

namespace FourthExercise.Tests.Unit
{
    [TestClass]
    public class EmployeeRepositoryTest
    {
        private const string testEmployeeName = "Pet";

        [TestCategory("Unit")]
        [TestMethod]
        public async Task Employee_FindWithName_InIsolation()
        {
            // Arrange

            List<JobRole> jobRoles = new List<JobRole>()
            {
                new JobRole() { Id = 1, Name = "Employee" },
                new JobRole() { Id = 2, Name = "Manager" },
                new JobRole() { Id = 3, Name = "Manager's Manager" }
            };

            var jobRolesQuerable = jobRoles.AsQueryable();

            var jobRolesMock = new Mock<DbSet<JobRole>>();

            jobRolesMock.As<IDbAsyncEnumerable<JobRole>>().Setup(m => m.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<JobRole>(jobRolesQuerable.GetEnumerator()));
            jobRolesMock.As<IQueryable<JobRole>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<Employee>(jobRolesQuerable.Provider));
            jobRolesMock.As<IQueryable<JobRole>>().Setup(m => m.Expression).Returns(jobRolesQuerable.Expression);
            jobRolesMock.As<IQueryable<JobRole>>().Setup(m => m.ElementType).Returns(jobRolesQuerable.ElementType);
            jobRolesMock.As<IQueryable<JobRole>>().Setup(m => m.GetEnumerator()).Returns(jobRolesQuerable.GetEnumerator());
            jobRolesMock.Setup(m => m.AsNoTracking()).Returns(jobRolesMock.Object);

            List<Employee> employees = new List<Employee>()
            {
                new Employee() { Id = 1, FirstName = "Petar", LastName = "Minkov", Email = "m_minkov@gmail.com", JobRoleId = 1, Salary = 3500 },
                new Employee() { Id = 2, FirstName = "Ivan", LastName = "Petrov", Email = "i_petrov@gmail.com", JobRoleId = 1, Salary = 3500 },
                new Employee() { Id = 3, FirstName = "Petar", LastName = "Ivanov", Email = "p_ivanov@gmail.com", JobRoleId = 2, Salary = 4000 },
                new Employee() { Id = 4, FirstName = "Dimitar", LastName = "Kirilov", Email = "d_kirilov@gmail.com", JobRoleId = 3, Salary = 4500 }
            };

            var employeesQuerable = employees.AsQueryable();

            var employeesMock = new Mock<DbSet<Employee>>();

            employeesMock.As<IDbAsyncEnumerable<Employee>>().Setup(m => m.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<Employee>(employeesQuerable.GetEnumerator()));
            employeesMock.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<Employee>(employeesQuerable.Provider));
            employeesMock.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(employeesQuerable.Expression);
            employeesMock.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(employeesQuerable.ElementType);
            employeesMock.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(employeesQuerable.GetEnumerator());
            employeesMock.Setup(m => m.Include(It.IsAny<string>())).Returns(employeesMock.Object);
            // employeesMock.Setup(m => m.Include(It.IsAny<Expression<Func<Employee, JobRole>>>())).Returns(employeesMock.Object);
            employeesMock.Setup(m => m.AsNoTracking()).Returns(employeesMock.Object);

            var contextMock = new Mock<FourthExerciseContext>();

            contextMock.Setup(m => m.JobRoles).Returns(jobRolesMock.Object);
            contextMock.Setup(m => m.Employees).Returns(employeesMock.Object);

            IUnitOfWorkFactory unitOfWorkFactory = new FourthExerciseUnitOfWorkFactory(contextMock.Object);
            IJobRoleMapper jobRoleMapper = new JobRoleMapper();
            IEmployeeMapper employeeMapper = new EmployeeMapper(jobRoleMapper);
            EmployeeRepository employeeRepository = new EmployeeRepository(contextMock.Object, employeeMapper);
            IReadEmployeeRepository readEmployeeRepository = employeeRepository;
            IWriteEmployeeRepository writeEmployeeRepository = employeeRepository;

            // Act

            IEnumerable<EmployeeModel> employeeModels = await readEmployeeRepository.FindWithNameAsync(testEmployeeName);

            // Assert

            bool all = employeeModels.All(em => em.FirstName.Contains(testEmployeeName) || em.LastName.Contains(testEmployeeName));

            Assert.IsTrue(all);
        }
    }
}
