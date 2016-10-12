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
    public class ChangeEmployeeService : IChangeEmployeeService
    {
        public ChangeEmployeeService(IUnitOfWorkFactory unitOfWorkFactory, IWriteEmployeeRepository writeEmployeeRepository)
        {
            if (unitOfWorkFactory == null) { throw new ArgumentNullException("unitOfWorkFactory"); }
            if (writeEmployeeRepository == null) { throw new ArgumentNullException("writeEmployeeRepository"); }

            this.unitOfWorkFactory = unitOfWorkFactory;
            this.writeEmployeeRepository = writeEmployeeRepository;
        }

        private IUnitOfWorkFactory unitOfWorkFactory;
        private IWriteEmployeeRepository writeEmployeeRepository;

        public async Task ChangeAsync(EmployeeModel employeeModel)
        {
            await unitOfWorkFactory.WithAsync(
                async uow =>
                {
                    await writeEmployeeRepository.SetAsync(employeeModel);

                    uow.Complete();
                }
            );
        }
    }
}
