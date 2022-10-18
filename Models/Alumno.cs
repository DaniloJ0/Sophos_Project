using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    [Table("alumno")]
    public partial class Alumno
    {
        public Alumno()
        {
            MatriculaAlumnos = new HashSet<MatriculaAlumno>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("nombre")]
        [StringLength(25)]
        [Unicode(false)]
        public string Nombre { get; set; } = null!;
        [Column("apellido")]
        [StringLength(25)]
        [Unicode(false)]
        public string Apellido { get; set; } = null!;
        [Column("credt_disp")]
        public int CredtDisp { get; set; }
        [Column("id_dept")]
        public int IdDept { get; set; }
        [Column("id_curso_realizado")]
        public int? IdCursoRealizado { get; set; }

        [ForeignKey("IdCursoRealizado")]
        [InverseProperty("Alumnos")]
        public virtual CursosRealizado? IdCursoRealizadoNavigation { get; set; }
        [ForeignKey("IdDept")]
        [InverseProperty("Alumnos")]
        public virtual Facultad IdDeptNavigation { get; set; } = null!;
        [InverseProperty("IdAlumnoNavigation")]
        public virtual ICollection<MatriculaAlumno> MatriculaAlumnos { get; set; }
    }
}
