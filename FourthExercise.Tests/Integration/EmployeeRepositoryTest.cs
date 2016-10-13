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

namespace FourthExercise.Tests.Integration
{
    [TestClass]
    public class EmployeeRepositoryTest
    {
        public EmployeeRepositoryTest()
        {
            jobRoleMapper = new JobRoleMapper();
            employeeMapper = new EmployeeMapper(jobRoleMapper);
        }

        private readonly int testEmployeeId = FourthExerciseSeed.Employees.First().Id;
        private const string testEmployeeName = "Pet";

        private FourthExerciseContext context;
        private IUnitOfWorkFactory unitOfWorkFactory;
        private IJobRoleMapper jobRoleMapper;
        private IEmployeeMapper employeeMapper;
        private IReadEmployeeRepository readEmployeeRepository;
        private IWriteEmployeeRepository writeEmployeeRepository;

        [TestInitialize]
        public void Initialize()
        {
            context = new FourthExerciseContext();
            unitOfWorkFactory = new FourthExerciseUnitOfWorkFactory(context);
            EmployeeRepository employeeRepository = new EmployeeRepository(context, employeeMapper);
            readEmployeeRepository = employeeRepository;
            writeEmployeeRepository = employeeRepository;
        }

        [TestMethod]
        public async Task Employee_FindWithName()
        {
            // Arrange

            // Act

            IEnumerable<EmployeeModel> employeeModels = await readEmployeeRepository.FindWithNameAsync(testEmployeeName);

            // Assert

            bool all = employeeModels.All(em => em.FirstName.Contains(testEmployeeName) || em.LastName.Contains(testEmployeeName));

            Assert.IsTrue(all);
        }


        [TestMethod]
        [TestCategory("Integration")]
        public async Task Employee_Get()
        {
            // Arrange

            // Act

            EmployeeModel employeeModel = await readEmployeeRepository.GetAsync(testEmployeeId);

            // Assert

            Assert.AreEqual(testEmployeeId, employeeModel.Id);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public async Task Employee_Set()
        {
            // Arrange

            EmployeeModel employeeModel = await readEmployeeRepository.GetAsync(testEmployeeId);

            employeeModel.FirstName = $"{employeeModel.FirstName}-{ Guid.NewGuid() }";

            // Act

            await unitOfWorkFactory.WithAsync(
                async uow =>
                {
                    await writeEmployeeRepository.SetAsync(employeeModel);

                    uow.Complete();
                }
            );

            // Assert

            EmployeeModel storedEmployeeModel = await readEmployeeRepository.GetAsync(testEmployeeId);

            Assert.AreEqual(storedEmployeeModel.FirstName, employeeModel.FirstName);
        }
    }
}
