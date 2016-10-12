using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FourthExercise.Models;
using FourthExercise.Infrastructure;
using FourthExercise.Infrastructure.Repositories;
using FourthExercise.Infrastructure.Entity;
using FourthExercise.Infrastructure.Entity.Repositories;

namespace FourthExercise.Tests
{
    [TestClass]
    public class EmployeeRepositoryTest
    {
        private const string name = "Z";

        private IUnitOfWorkFactory unitOfWorkFactory;
        private IReadEmployeeRepository readEmployeeRepository;
        private IWriteEmployeeRepository writeEmployeeRepository;

        [TestInitialize]
        public void Initialize()
        {
            FourthExerciseContext context = new FourthExerciseContext();

            unitOfWorkFactory = new FourthExerciseUnitOfWorkFactory(context);

            EmployeeRepository employeeRepository = new EmployeeRepository(context);

            readEmployeeRepository = employeeRepository;
            writeEmployeeRepository = employeeRepository;
        }

        [TestMethod]
        public async Task TestFindWithName()
        {
            IEnumerable<EmployeeModel> employeeModels = await readEmployeeRepository.FindWithNameAsync(name);

            Assert.AreEqual(0, employeeModels.ToList().Count);
        }

        [TestMethod]
        public async Task TestAdd()
        {
            EmployeeModel employeeModel = new EmployeeModel();

            await unitOfWorkFactory.WithAsync(
                async uow =>
                {
                    await writeEmployeeRepository.AddAsync(employeeModel);

                    uow.Complete();
                }
            );

            // ...
        }

        [TestMethod]
        public async Task TestSet()
        {
            EmployeeModel employeeModel = new EmployeeModel();

            await unitOfWorkFactory.WithAsync(
                async uow =>
                {
                    await writeEmployeeRepository.SetAsync(employeeModel);

                    uow.Complete();
                }
            );

            // ...
        }

        [TestMethod]
        public async Task TestRemove()
        {
            EmployeeModel employeeModel = new EmployeeModel();

            await unitOfWorkFactory.WithAsync(
                async uow =>
                {
                    await writeEmployeeRepository.RemoveAsync(employeeModel);

                    uow.Complete();
                }
            );

            // ...
        }
    }
}
