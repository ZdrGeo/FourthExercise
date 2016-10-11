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
    public class EmployeeRepository : EnlistableRepository<FourthExerciseContext>, IEmployeeRepository
    {
        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await UnitOfWork
                .Employees
                .Include(e => e.JobRole)
                .ToListAsync();
        }

        public Task<Employee> GetAsync(int id)
        {
            return UnitOfWork
                .Employees
                .Include(e => e.JobRole)
                .SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Employee>> FindWithNameAsync(string name)
        {
            return await UnitOfWork
                .Employees
                .Include(e => e.JobRole)
                .Where(e => e.FirstName.Contains(name) || e.LastName.Contains(name))
                .ToListAsync();
        }

        public Task AddAsync(Employee employee)
        {
            UnitOfWork.Employees.Add(employee);

            return Task.FromResult(1);
        }

        public Task SetAsync(Employee employee)
        {
            UnitOfWork.Entry(employee).State = EntityState.Modified;

            return Task.FromResult(1);
        }

        public Task RemoveAsync(Employee employee)
        {
            UnitOfWork.Employees.Remove(employee);

            return Task.FromResult(1);
        }
    }
}
