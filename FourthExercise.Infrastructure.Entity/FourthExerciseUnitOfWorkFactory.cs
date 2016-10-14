using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;
using System.Transactions;
using System.Data.Entity.Infrastructure;

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

        private void RejectChanges()
        {
            foreach (DbEntityEntry entry in context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                    default: break;
                }
            }
        }

        public async Task WithAsync(Func<UnitOfWork, Task> action)
        {
            UnitOfWork unitOfWork = new UnitOfWork();

            await action(unitOfWork);

            if (unitOfWork.IsCompleted)
            {
                await context.SaveChangesAsync();
            }
            else
            {
                RejectChanges();
            }

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
                else
                {
                    RejectChanges();
                }
            }
            */
        }
    }
}
