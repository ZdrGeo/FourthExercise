using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FourthExercise.Models;

namespace FourthExercise.Services
{
    public interface IChangeEmployeeService
    {
        Task ChangeAsync(EmployeeModel employeeModel);
    }
}
