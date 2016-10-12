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
        private IReadJobRoleRepository readJobRoleRepository;

        [TestInitialize]
        public void Initialize()
        {
            FourthExerciseContext context = new FourthExerciseContext();

            readJobRoleRepository = new JobRoleRepository(context);
        }

        [TestMethod]
        public async Task TestGetAll()
        {
            IEnumerable<JobRoleModel> jobRoleModels = await readJobRoleRepository.GetAllAsync();

            Assert.AreEqual(3, jobRoleModels.ToList().Count);
        }
    }
}
