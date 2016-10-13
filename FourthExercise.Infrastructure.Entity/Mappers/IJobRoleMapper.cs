using FourthExercise.Infrastructure.Entity.Models;
using FourthExercise.Models;

namespace FourthExercise.Infrastructure.Entity.Mappers
{
    public interface IJobRoleMapper
    {
        JobRoleModel MapJobRoleToModel(JobRole jobRole);
        JobRole MapModelToJobRole(JobRoleModel jobRoleModel);
    }
}