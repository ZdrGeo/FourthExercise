using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FourthExercise.Models;
using FourthExercise.Infrastructure;
using FourthExercise.Infrastructure.Repositories;
using FourthExercise.Infrastructure.Entity;
using FourthExercise.Infrastructure.Entity.Mappers;
using FourthExercise.Infrastructure.Entity.Repositories;

namespace FourthExercise.Tests.Integration
{
    [TestClass]
    public class JobRoleRepositoryTest
    {
        public JobRoleRepositoryTest()
        {
            jobRoleMapper = new JobRoleMapper();
        }

        private FourthExerciseContext context;
        private IJobRoleMapper jobRoleMapper;
        private IReadJobRoleRepository readJobRoleRepository;

        [TestInitialize]
        public void Initialize()
        {
            context = new FourthExerciseContext();
            readJobRoleRepository = new JobRoleRepository(context, jobRoleMapper);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public async Task TestGetAll()
        {
            IEnumerable<JobRoleModel> jobRoleModels = await readJobRoleRepository.GetAllAsync();

            Assert.AreEqual(FourthExerciseSeed.JobRoles.Count, jobRoleModels.ToList().Count);
        }
    }
}
