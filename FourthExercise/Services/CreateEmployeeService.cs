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
    public class CreateEmployeeService : ICreateEmployeeService
    {
        public CreateEmployeeService(IUnitOfWorkFactory unitOfWorkFactory, IWriteEmployeeRepository writeEmployeeRepository)
        {
            Check.NotNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            Check.NotNull(writeEmployeeRepository, nameof(writeEmployeeRepository));

            this.unitOfWorkFactory = unitOfWorkFactory;
            this.writeEmployeeRepository = writeEmployeeRepository;
        }

        private IUnitOfWorkFactory unitOfWorkFactory;
        private IWriteEmployeeRepository writeEmployeeRepository;

        public async Task CreateAsync(EmployeeModel employeeModel)
        {
            await unitOfWorkFactory.WithAsync(
                async uow =>
                {
                    await writeEmployeeRepository.AddAsync(employeeModel);

                    uow.Complete();
                }
            );
        }
    }
}
