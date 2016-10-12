using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FourthExercise.Models;

namespace FourthExercise.Services
{
    public interface IDeleteEmployeeService
    {
        Task DeleteAsync(Employee employee);
    }
}
