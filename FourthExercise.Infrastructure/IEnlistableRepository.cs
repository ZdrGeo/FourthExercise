using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourthExercise.Infrastructure
{
    public interface IEnlistableRepository
    {
        void Enlist(IUnitOfWork unitOfWork);
        void Delist();
    }
}
