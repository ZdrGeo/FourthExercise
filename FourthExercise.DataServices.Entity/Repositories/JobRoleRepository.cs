using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;

using FourthExercise.Models;
using FourthExercise.DataServices;
using FourthExercise.DataServices.Repositories;

namespace FourthExercise.DataServices.Entity.Repositories
{
    public class JobRoleRepository : EnlistableRepository<FourthExerciseContext>, IJobRoleRepository
    {
        public async Task<IEnumerable<JobRole>> GetAllAsync()
        {
            return await UnitOfWork.JobRoles.ToListAsync();
        }
    }
}
