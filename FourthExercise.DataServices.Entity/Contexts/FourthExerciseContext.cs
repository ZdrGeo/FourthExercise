using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

using FourthExercise.Models;

namespace FourthExercise.DataServices.Entity
{
    public class FourthExerciseContext : DbContext
    {
        public FourthExerciseContext() : base("name=FourthExerciseContext")
        {
            Database.SetInitializer<FourthExerciseContext>(new FourthExerciseInitializer());
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<JobRole> JobRoles { get; set; }
    }
}
