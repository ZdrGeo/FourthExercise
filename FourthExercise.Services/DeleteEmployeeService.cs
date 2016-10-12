using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FourthExercise.Models;
using FourthExercise.DataServices;
using FourthExercise.DataServices.Repositories;

namespace FourthExercise.Services
{
    public class DeleteEmployeeService : IDeleteEmployeeService
    {
        public DeleteEmployeeService(IUnitOfWorkFactory unitOfWorkFactory, IEmployeeRepository employeeRepository)
        {
            if (unitOfWorkFactory == null) { throw new ArgumentNullException("unitOfWorkFactory"); }
            if (employeeRepository == null) { throw new ArgumentNullException("employeeRepository"); }

            this.unitOfWorkFactory = unitOfWorkFactory;
            this.employeeRepository = employeeRepository;
        }

        private IUnitOfWorkFactory unitOfWorkFactory;
        private IEmployeeRepository employeeRepository;

        public async Task DeleteAsync(Employee employee)
        {
            await unitOfWorkFactory.WithAsync(
                async uow =>
                {
                    employeeRepository.Enlist(uow);
                    await employeeRepository.RemoveAsync(employee);
                    await uow.CompleteAsync();
                    employeeRepository.Delist();
                }
            );
        }
    }
}
