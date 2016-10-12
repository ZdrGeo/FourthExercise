using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourthExercise.Infrastructure.Entity
{
    public class FourthExerciseUnitOfWork : IUnitOfWork
    {
        public FourthExerciseUnitOfWork(FourthExerciseContext context)
        {
            if (context == null) { throw new ArgumentNullException("context"); }

            this.context = context;
        }

        private FourthExerciseContext context;

        public object Get()
        {
            return context;
        }

        public Task CompleteAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}
