using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourthExercise.Infrastructure
{
    public abstract class EnlistableRepository<TUnitOfWork> : IEnlistableRepository where TUnitOfWork : class
    {
        protected TUnitOfWork UnitOfWork { get; private set; }

        protected void EnsureEnlisted()
        {
            if (UnitOfWork == null) { throw new InvalidOperationException("Repository is not enlisted in unit of work"); }
        }

        public void Enlist(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null) { throw new ArgumentNullException("unitOfWork", "Repository can not be enlisted in null unit of work"); }

            UnitOfWork = (TUnitOfWork)unitOfWork.Get();
        }

        public void Delist()
        {
            UnitOfWork = null;
        }
    }
}
