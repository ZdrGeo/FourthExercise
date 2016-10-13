using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;

using FourthExercise.Models;
using FourthExercise.Infrastructure.Repositories;
using FourthExercise.Infrastructure.Entity.Models;

namespace FourthExercise.Infrastructure.Entity.Mappers
{
    public class JobRoleMapper : IJobRoleMapper
    {
        public JobRoleModel MapJobRoleToModel(JobRole jobRole)
        {
            if (jobRole == null) { return null; }

            JobRoleModel jobRoleModel = new JobRoleModel();

            jobRoleModel.Id = jobRole.Id;
            jobRoleModel.Name = jobRole.Name;

            return jobRoleModel;
        }

        public JobRole MapModelToJobRole(JobRoleModel jobRoleModel)
        {
            if (jobRoleModel == null) { return null; }

            JobRole jobRole = new JobRole();

            jobRole.Id = jobRoleModel.Id;
            jobRole.Name = jobRoleModel.Name;

            return jobRole;
        }
    }
}
