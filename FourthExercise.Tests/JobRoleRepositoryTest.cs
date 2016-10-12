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
    public class JobRoleRepositoryTest
    {
        private IUnitOfWorkFactory unitOfWorkFactory;
        private IReadJobRoleRepository readJobRoleRepository;

        [TestInitialize]
        public void Initialize()
        {
            unitOfWorkFactory = new FourthExerciseUnitOfWorkFactory();
            readJobRoleRepository = new JobRoleRepository();
        }

        [TestMethod]
        public async Task TestChange()
        {
            IEnumerable<JobRoleModel> jobRoleModels = new List<JobRoleModel>();

            await unitOfWorkFactory.WithAsync(
                async uow =>
                {
                    readJobRoleRepository.Enlist(uow);
                    jobRoleModels = await readJobRoleRepository.GetAllAsync();
                    readJobRoleRepository.Delist();
                }
            );

            Assert.AreEqual(3, jobRoleModels.ToList().Count);
        }
    }
}
