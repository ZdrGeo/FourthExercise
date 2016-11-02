using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FourthExercise.Infrastructure.Entity.Models
{
    public class JobRole
    {
        [Key]
        public int Id { get; set; }
        [StringLength(128)]
        public string Name { get; set; }
    }
}