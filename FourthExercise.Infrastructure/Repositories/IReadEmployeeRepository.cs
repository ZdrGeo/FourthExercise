using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FourthExercise.Models;
using FourthExercise.Infrastructure;

namespace FourthExercise.Infrastructure.Repositories
{
    public interface IReadEmployeeRepository : IEnlistableRepository
    {
        Task<IEnumerable<Employee>> FindWithNameAsync(string name);
        Task<Employee> GetAsync(int id);
    }
}
