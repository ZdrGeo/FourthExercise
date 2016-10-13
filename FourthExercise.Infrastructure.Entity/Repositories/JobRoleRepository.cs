using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;

using FourthExercise.Models;
using FourthExercise.Infrastructure.Repositories;
using FourthExercise.Infrastructure.Entity.Mappers;

namespace FourthExercise.Infrastructure.Entity.Repositories
{
    public class JobRoleRepository : IReadJobRoleRepository
    {
        public JobRoleRepository(FourthExerciseContext context, IJobRoleMapper jobRoleMapper)
        {
            if (context == null) { throw new ArgumentNullException("context"); }
            if (jobRoleMapper == null) { throw new ArgumentNullException("jobRoleMapper"); }

            this.context = context;
            this.jobRoleMapper = jobRoleMapper;
        }

        private FourthExerciseContext context;
        private IJobRoleMapper jobRoleMapper;

        public async Task<IEnumerable<JobRoleModel>> GetAllAsync()
        {
            return (await context.JobRoles.AsNoTracking().ToListAsync()).Select(jr => jobRoleMapper.MapJobRoleToModel(jr));
        }
    }
}
