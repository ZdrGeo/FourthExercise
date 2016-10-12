using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FourthExercise.Models;

namespace FourthExercise.Infrastructure.Repositories
{
    public interface IWriteEmployeeRepository : IEnlistableRepository
    {
        Task AddAsync(Employee employee);
        Task SetAsync(Employee employee);
        Task RemoveAsync(Employee employee);
    }
}
