using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourthExercise.Infrastructure
{
    public class UnitOfWork
    {
        private bool isCompleted;

        public bool IsCompleted => isCompleted;

        public void Complete()
        {
            isCompleted = true;
        }
    }
}
