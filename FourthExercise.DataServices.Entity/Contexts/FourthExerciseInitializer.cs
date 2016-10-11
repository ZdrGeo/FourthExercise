using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

using FourthExercise.Models;

namespace FourthExercise.DataServices.Entity
{
    public class FourthExerciseInitializer : DropCreateDatabaseIfModelChanges<FourthExerciseContext> // DropCreateDatabaseIfModelChanges<FourthExerciseContext> // DropCreateDatabaseAlways<FourthExerciseContext>
    {
        protected override void Seed(FourthExerciseContext context)
        {
            List<JobRole> jobRoles = new List<JobRole>();

            jobRoles.Add(new JobRole() { Id = 1, Name = "Employee" });
            jobRoles.Add(new JobRole() { Id = 2, Name = "Manager" });
            jobRoles.Add(new JobRole() { Id = 3, Name = "Manager's Manager" });

            context.JobRoles.AddRange(jobRoles);

            base.Seed(context);
        }
    }
}