using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FourthExercise.Models
{
    public class JobRole
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Job Name")]
        public string Name { get; set; }
    }
}