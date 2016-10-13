using FourthExercise.Infrastructure.Entity.Models;
using FourthExercise.Models;

namespace FourthExercise.Infrastructure.Entity.Mappers
{
    public interface IEmployeeMapper
    {
        EmployeeModel MapEmployeeToModel(Employee employee);
        Employee MapModelToEmployee(EmployeeModel employeeModel);
    }
}