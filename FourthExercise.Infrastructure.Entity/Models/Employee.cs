using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FourthExercise.Infrastructure.Entity.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [StringLength(64)]
        public string FirstName { get; set; }
        [StringLength(64)]
        public string LastName { get; set; }
        [StringLength(256)]
        public string Email { get; set; }
        [ForeignKey("JobRole")]
        public int JobRoleId { get; set; }
        public JobRole JobRole { get; set; }
        public decimal Salary { get; set; }
    }
}