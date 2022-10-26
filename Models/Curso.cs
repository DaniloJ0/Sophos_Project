using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    [Table("curso")]
    public partial class Curso
    {
        public Curso()
        {
            MatriculaAlumnos = new HashSet<MatriculaAlumno>();
            //add
            CursosRealizados = new HashSet<CursosRealizado>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("nombre")]
        [StringLength(50)]
        [Unicode(false)]
        public string Nombre { get; set; } = null!;
        [Column("creditos")]
        public int Creditos { get; set; }
        [Column("cupos")]
        public int Cupos { get; set; }
        [Column("id_profesor")]
        public int? IdProfesor { get; set; }
        [Column("id_periodo")]
        public int IdPeriodo { get; set; }
        [Column("id_curso_pre")]
        public int? IdCursoPre { get; set; }

        [ForeignKey("IdPeriodo")]
        [InverseProperty("Cursos")]
        public virtual Periodo? IdPeriodoNavigation { get; set; }
        [ForeignKey("IdProfesor")]
        [InverseProperty("Cursos")]
        public virtual Profesor? IdProfesorNavigation { get; set; }

        [InverseProperty("IdCursoNavigation")]
        public virtual ICollection<MatriculaAlumno> MatriculaAlumnos { get; set; }

        //add
        [InverseProperty("IdCursoNavigation")]
        [JsonIgnore]
        public virtual ICollection<CursosRealizado> CursosRealizados { get; set; }
    }
}
