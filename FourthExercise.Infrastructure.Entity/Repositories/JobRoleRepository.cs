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
using FourthExercise.Infrastructure.Entity.Mappers;

namespace FourthExercise.Infrastructure.Entity.Repositories
{
    public class JobRoleRepository : EnlistableRepository<FourthExerciseContext>, IReadJobRoleRepository
    {
        public async Task<IEnumerable<JobRoleModel>> GetAllAsync()
        {
            return (await UnitOfWork.JobRoles.ToListAsync()).Select(jr => JobRoleMapper.MapJobRoleToModel(jr));
        }
    }
}
