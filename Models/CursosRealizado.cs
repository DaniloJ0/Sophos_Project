using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    [Table("cursos_realizados")]
    public partial class CursosRealizado
    {
        public CursosRealizado()
        {
            Alumnos = new HashSet<Alumno>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("id_alumno")]
        public int IdAlumno { get; set; }
        [Column("id_curso")]
        public int IdCurso { get; set; }

        [InverseProperty("IdCursoRealizadoNavigation")]
        public virtual ICollection<Alumno> Alumnos { get; set; }
    }
}
