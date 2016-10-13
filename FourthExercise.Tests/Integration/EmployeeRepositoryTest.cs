using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using FourthExercise.Models;
using FourthExercise.Infrastructure;
using FourthExercise.Infrastructure.Repositories;
using FourthExercise.Infrastructure.Entity;
using FourthExercise.Infrastructure.Entity.Models;
using FourthExercise.Infrastructure.Entity.Repositories;
using FourthExercise.Tests.Msdn;
using System.Data.Entity.Infrastructure;

namespace FourthExercise.Tests.Integration
{
    public enum SeededJobRoleId
    {
        Employee = 1,
        Manager = 2,
        ManagersManager = 3
    }

    [TestClass]
    public class EmployeeRepositoryTest
    {
        private const int seededEmployeeId = 1;
        private const string name = "Z";

        private IUnitOfWorkFactory unitOfWorkFactory;
        private IReadEmployeeRepository readEmployeeRepository;
        private IWriteEmployeeRepository writeEmployeeRepository;

        [TestInitialize]
        public void Initialize()
        {
            FourthExerciseContext context = new FourthExerciseContext();

            unitOfWorkFactory = new FourthExerciseUnitOfWorkFactory(context);
            readEmployeeRepository = new EmployeeRepository(context);
            writeEmployeeRepository = new EmployeeRepository(context);
        }

        [TestMethod]
        [Ignore]
        public async Task TestFindWithName()
        {
            EmployeeModel employeeModel = await readEmployeeRepository.GetAsync(seededEmployeeId);

            IEnumerable<EmployeeModel> employeeModels = await readEmployeeRepository.FindWithNameAsync(name);

            Assert.AreEqual(seededEmployeeId, employeeModel.Id);
            // Assert.AreEqual(0, employeeModels.ToList().Count);
        }


        [TestMethod]
        [TestCategory("Integration")]
        public async Task TestGet()
        {
            EmployeeModel employeeModel = await readEmployeeRepository.GetAsync(seededEmployeeId);

            Assert.AreEqual(seededEmployeeId, employeeModel.Id);
        }

        [TestCategory("Integration")]
        [TestMethod]
        [Ignore]
        public async Task TestSet()
        {
            EmployeeModel employeeModel = new EmployeeModel()
            {
                Id = seededEmployeeId, FirstName = "Zdravko", LastName = "Georgiev", Email = "z_georgiev@applss.com", JobRoleId = (int)SeededJobRoleId.Employee, Salary = 4600
            };

            employeeModel.Id = 1;
            await unitOfWorkFactory.WithAsync(
                async uow =>
                {
                    await writeEmployeeRepository.SetAsync(employeeModel);

                    uow.Complete();
                }
            );

            // EmployeeModel employeeModel = await readEmployeeRepository.GetAsync(seededEmployeeId);
        }
    }
}
