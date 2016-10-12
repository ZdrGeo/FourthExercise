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

        [TestInitialize]
        public void Initialize()
        {
            unitOfWorkFactory = new FourthExerciseUnitOfWorkFactory();
            readEmployeeRepository = new EmployeeRepository();
        }

        [TestMethod]
        public async Task TestChange()
        {
            IEnumerable<EmployeeModel> employeeModels = new List<EmployeeModel>();

            await unitOfWorkFactory.WithAsync(
                async uow =>
                {
                    readEmployeeRepository.Enlist(uow);
                    employeeModels = await readEmployeeRepository.FindWithNameAsync(name);
                    readEmployeeRepository.Delist();
                }
            );

            Assert.AreEqual(0, employeeModels.ToList().Count);
        }
    }
}
