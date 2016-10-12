using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;

using FourthExercise.Models;
using FourthExercise.Infrastructure;
using FourthExercise.Infrastructure.Repositories;

namespace FourthExercise.Infrastructure.Entity.Repositories
{
    public class JobRoleRepository : EnlistableRepository<FourthExerciseContext>, IReadJobRoleRepository
    {
        public async Task<IEnumerable<JobRole>> GetAllAsync()
        {
            return await UnitOfWork.JobRoles.ToListAsync();
        }
    }
}
