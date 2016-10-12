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
    public class EmployeeRepository : EnlistableRepository<FourthExerciseContext>, IReadEmployeeRepository, IWriteEmployeeRepository
    {
        public async Task<IEnumerable<EmployeeModel>> FindWithNameAsync(string name)
        {
            var employees = UnitOfWork.Employees.Include(e => e.JobRole);

            if (name != null)
            {
                employees = employees.Where(e => e.FirstName.Contains(name) || e.LastName.Contains(name));
            }
                
            return (await employees.ToListAsync()).Select(e => EmployeeMapper.MapEmployeeToModel(e));
        }

        public async Task<EmployeeModel> GetAsync(int id)
        {
            return EmployeeMapper.MapEmployeeToModel(await UnitOfWork.Employees.Include(e => e.JobRole).SingleOrDefaultAsync(e => e.Id == id));
        }

        public Task AddAsync(EmployeeModel employeeModel)
        {
            UnitOfWork.Employees.Add(EmployeeMapper.MapModelToEmployee(employeeModel));

            return Task.CompletedTask;
        }

        public Task SetAsync(EmployeeModel employeeModel)
        {
            UnitOfWork.Entry(EmployeeMapper.MapModelToEmployee(employeeModel)).State = EntityState.Modified;

            return Task.CompletedTask;
        }

        public Task RemoveAsync(EmployeeModel employeeModel)
        {
            Employee employee = EmployeeMapper.MapModelToEmployee(employeeModel);

            UnitOfWork.Employees.Attach(employee);
            UnitOfWork.Employees.Remove(employee);

            return Task.CompletedTask;
        }
    }
}
