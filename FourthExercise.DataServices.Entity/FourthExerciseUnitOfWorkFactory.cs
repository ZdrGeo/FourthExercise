using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;

namespace FourthExercise.DataServices.Entity
{
    public class FourthExerciseUnitOfWorkFactory : IUnitOfWorkFactory
    {
        public async Task WithAsync(Func<IUnitOfWork, Task> action)
        {
            using (FourthExerciseContext context = new FourthExerciseContext())
            {
                /*
                using (DbContextTransaction dbContextTransaction = context.Database.BeginTransaction())
                {
                    FourthExerciseUnitOfWork unitOfWork = new FourthExerciseUnitOfWork(context);

                    await action(unitOfWork);
                }
                */

                FourthExerciseUnitOfWork unitOfWork = new FourthExerciseUnitOfWork(context);

                await action(unitOfWork);
            }
        }
    }
}
