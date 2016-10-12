using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourthExercise.Infrastructure
{
    public interface IUnitOfWork
    {
        object Get();
        Task CompleteAsync();
    }
}
