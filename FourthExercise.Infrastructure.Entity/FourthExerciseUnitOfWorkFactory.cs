using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;

namespace FourthExercise.Infrastructure.Entity
{
    public class FourthExerciseUnitOfWorkFactory : IUnitOfWorkFactory
    {
        public async Task WithAsync(Func<IUnitOfWork, Task> action)
        {
            using (FourthExerciseContext context = new FourthExerciseContext())
            {
                FourthExerciseUnitOfWork unitOfWork = new FourthExerciseUnitOfWork(context);

                await action(unitOfWork);
            }
        }
    }
}
