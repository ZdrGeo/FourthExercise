using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FourthExercise.DataServices;
using FourthExercise.Models;

namespace FourthExercise.DataServices.Repositories
{
    public interface IEmployeeRepository : IEnlistableRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee> GetAsync(int id);
        // Task<Employee> FindWithFirstNameAsync(string firstName);
        // Task<Employee> FindWithLastNameAsync(string lastName);
        Task AddAsync(Employee employee);
        Task SetAsync(Employee employee);
        Task RemoveAsync(Employee employee);
    }
}
