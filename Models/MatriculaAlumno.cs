using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    [Table("matricula_alumno")]
    public partial class MatriculaAlumno
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("id_curso")]
        public int IdCurso { get; set; }
        [Column("id_alumno")]
        public int IdAlumno { get; set; }

        [ForeignKey("IdAlumno")]
        [InverseProperty("MatriculaAlumnos")]
        [JsonIgnore]
        public virtual Alumno? IdAlumnoNavigation { get; set; } = null!;
        [ForeignKey("IdCurso")]
        [InverseProperty("MatriculaAlumnos")]
        [JsonIgnore]
        public virtual Curso? IdCursoNavigation { get; set; } = null!;
    }
}
