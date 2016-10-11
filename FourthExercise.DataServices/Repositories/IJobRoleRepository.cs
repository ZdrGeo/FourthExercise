using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FourthExercise.DataServices;
using FourthExercise.Models;

namespace FourthExercise.DataServices.Repositories
{
    public interface IJobRoleRepository : IEnlistableRepository
    {
        Task<IEnumerable<JobRole>> GetAllAsync();
    }
}
