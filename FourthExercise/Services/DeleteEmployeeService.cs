using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FourthExercise.Models;
using FourthExercise.Infrastructure;
using FourthExercise.Infrastructure.Repositories;
using Checks;

namespace FourthExercise.Services
{
    public class DeleteEmployeeService : IDeleteEmployeeService
    {
        public DeleteEmployeeService(IUnitOfWorkFactory unitOfWorkFactory, IWriteEmployeeRepository writeEmployeeRepository)
        {
            Check.NotNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            Check.NotNull(writeEmployeeRepository, nameof(writeEmployeeRepository));

            this.unitOfWorkFactory = unitOfWorkFactory;
            this.writeEmployeeRepository = writeEmployeeRepository;
        }

        private IUnitOfWorkFactory unitOfWorkFactory;
        private IWriteEmployeeRepository writeEmployeeRepository;

        public async Task DeleteAsync(EmployeeModel employeeModel)
        {
            await unitOfWorkFactory.WithAsync(
                async uow =>
                {
                    await writeEmployeeRepository.RemoveAsync(employeeModel);

                    uow.Complete();
                }
            );
        }
    }
}
