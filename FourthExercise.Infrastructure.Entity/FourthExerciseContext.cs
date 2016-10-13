using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

using FourthExercise.Infrastructure.Entity.Models;

namespace FourthExercise.Infrastructure.Entity
{
    public class FourthExerciseContext : DbContext
    {
        public FourthExerciseContext() : base("name=FourthExercise")
        {
            Database.SetInitializer(new FourthExerciseInitializer());
        }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<JobRole> JobRoles { get; set; }
    }
}
