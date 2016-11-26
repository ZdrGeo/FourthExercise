using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;

using Dapper;

namespace FourthExercise.Infrastructure.Dapper
{
    public class FourthExerciseUnitOfWorkFactory : IUnitOfWorkFactory
    {
        public FourthExerciseUnitOfWorkFactory(DbConnection dbConnection)
        {
            if (dbConnection == null) { throw new ArgumentNullException("context"); }

            this.dbConnection = dbConnection;
        }

        private DbConnection dbConnection;

        public async Task WithAsync(Func<UnitOfWork, Task> action)
        {
            bool close = await dbConnection.OpenIfNeededAsync();

            /*
            using (DbTransaction dbTransaction = dbConnection.BeginTransaction())
            {
                try
                {
                    UnitOfWork unitOfWork = new UnitOfWork();

                    await action(unitOfWork);

                    if (unitOfWork.IsCompleted) { dbTransaction.Commit(); } else { dbTransaction.Rollback(); }
                }
                catch
                {
                    try { dbTransaction.Rollback(); } catch { } throw;
                }
            }
            */

            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                dbConnection.EnlistTransaction(Transaction.Current);

                var unitOfWork = new UnitOfWork();

                await action(unitOfWork);

                if (unitOfWork.IsCompleted) { transactionScope.Complete(); }
            }

            if (close) { dbConnection.Close(); }
        }
    }
}
