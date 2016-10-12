using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FourthExercise.Models;
using FourthExercise.Infrastructure;
using FourthExercise.Infrastructure.Repositories;

namespace FourthExercise.Services
{
    public class CreateEmployeeService : ICreateEmployeeService
    {
        public CreateEmployeeService(IUnitOfWorkFactory unitOfWorkFactory, IWriteEmployeeRepository employeeRepository)
        {
            if (unitOfWorkFactory == null) { throw new ArgumentNullException("unitOfWorkFactory"); }
            if (employeeRepository == null) { throw new ArgumentNullException("employeeRepository"); }

            this.unitOfWorkFactory = unitOfWorkFactory;
            this.employeeRepository = employeeRepository;
        }

        private IUnitOfWorkFactory unitOfWorkFactory;
        private IWriteEmployeeRepository employeeRepository;

        public async Task CreateAsync(Employee employee)
        {
            await unitOfWorkFactory.WithAsync(
                async uow =>
                {
                    employeeRepository.Enlist(uow);
                    await employeeRepository.AddAsync(employee);
                    await uow.CompleteAsync();
                    employeeRepository.Delist();
                }
            );
        }
    }
}
