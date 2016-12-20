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
using Checks;

namespace FourthExercise.Infrastructure.Entity.Repositories
{
    public class JobRoleRepository : IReadJobRoleRepository
    {
        public JobRoleRepository(FourthExerciseContext context, IJobRoleMapper jobRoleMapper)
        {
            Check.NotNull(context, nameof(context));
            Check.NotNull(jobRoleMapper, nameof(jobRoleMapper));

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
