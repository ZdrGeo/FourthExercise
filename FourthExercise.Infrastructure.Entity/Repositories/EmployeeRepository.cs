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
    public class EmployeeRepository : IReadEmployeeRepository, IWriteEmployeeRepository
    {
        public EmployeeRepository(FourthExerciseContext context, IEmployeeMapper employeeMapper)
        {
            if (context == null) { throw new ArgumentNullException("context"); }
            if (employeeMapper == null) { throw new ArgumentNullException("employeeMapper"); }

            this.context = context;
            this.employeeMapper = employeeMapper;
        }

        private FourthExerciseContext context;
        private IEmployeeMapper employeeMapper;

        public async Task<IEnumerable<EmployeeModel>> FindWithNameAsync(string name)
        {
            var employees = context.Employees.Include(e => e.JobRole);

            if (name != null)
            {
                employees = employees.Where(e => e.FirstName.Contains(name) || e.LastName.Contains(name));
            }
                
            return (await employees.AsNoTracking().ToListAsync()).Select(e => employeeMapper.MapEmployeeToModel(e));
        }

        public async Task<EmployeeModel> GetAsync(int id)
        {
            return employeeMapper.MapEmployeeToModel(await context.Employees.Include(e => e.JobRole).AsNoTracking().SingleOrDefaultAsync(e => e.Id == id));
        }

        public Task AddAsync(EmployeeModel employeeModel)
        {
            context.Employees.Add(employeeMapper.MapModelToEmployee(employeeModel));

            return Task.CompletedTask;
        }

        public Task SetAsync(EmployeeModel employeeModel)
        {
            context.Entry(employeeMapper.MapModelToEmployee(employeeModel)).State = EntityState.Modified;

            return Task.CompletedTask;
        }

        public Task RemoveAsync(EmployeeModel employeeModel)
        {
            Employee employee = employeeMapper.MapModelToEmployee(employeeModel);

            context.Employees.Attach(employee);
            context.Employees.Remove(employee);

            return Task.CompletedTask;
        }
    }
}
