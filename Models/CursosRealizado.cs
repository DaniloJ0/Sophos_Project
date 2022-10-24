using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    [Table("cursos_realizados")]
    public partial class CursosRealizado
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("id_alumno")]
        public int IdAlumno { get; set; }
        [Column("id_curso")]
        public int IdCurso { get; set; }

        [ForeignKey("IdAlumno")]
        [InverseProperty("CursosRealizados")]
        //[JsonIgnore]
        public virtual Alumno? IdAlumnoNavigation { get; set; }

        [ForeignKey("IdCurso")]
        [InverseProperty("CursosRealizados")]
        //[JsonIgnore]
        public virtual Curso? IdCursoNavigation { get; set; } = null!;
    }
}
