using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    [Table("facultad")]
    public partial class Facultad
    {
        public Facultad()
        {
            Alumnos = new HashSet<Alumno>();
            Profesors = new HashSet<Profesor>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        [StringLength(80)]
        [Unicode(false)]
        public string Name { get; set; } = null!;

        [InverseProperty("IdDeptNavigation")]
        public virtual ICollection<Alumno> Alumnos { get; set; }
        [InverseProperty("IdDeptNavigation")]
        public virtual ICollection<Profesor> Profesors { get; set; }
    }
}
