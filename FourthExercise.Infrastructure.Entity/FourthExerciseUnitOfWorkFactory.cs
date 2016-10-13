using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;
using System.Transactions;

namespace FourthExercise.Infrastructure.Entity
{
    public class FourthExerciseUnitOfWorkFactory : IUnitOfWorkFactory
    {
        public FourthExerciseUnitOfWorkFactory(FourthExerciseContext context)
        {
            if (context == null) { throw new ArgumentNullException("context"); }

            this.context = context;
        }

        private FourthExerciseContext context;

        public async Task WithAsync(Func<UnitOfWork, Task> action)
        {
            UnitOfWork unitOfWork = new UnitOfWork();

            await action(unitOfWork);

            if (unitOfWork.IsCompleted) { await context.SaveChangesAsync(); }

            // This should be the implementation in case that we need distributed transaction
            /*
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                UnitOfWork unitOfWork = new UnitOfWork();

                await action(unitOfWork);

                if (unitOfWork.IsCompleted)
                {
                    await context.SaveChangesAsync();

                    transactionScope.Complete();
                }
            }
            */
        }
    }
}
