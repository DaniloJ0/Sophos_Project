using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    [Table("alumno")]
    public partial class Alumno
    {
        public Alumno()
        {
            CursosRealizados = new HashSet<CursosRealizado>();
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

        [ForeignKey("IdDept")]
        [InverseProperty("Alumnos")]
        [JsonIgnore]
        public virtual Facultad? IdDeptNavigation { get; set; } = null!;
        [InverseProperty("IdAlumnoNavigation")]
        [JsonIgnore]
        public virtual ICollection<CursosRealizado> CursosRealizados { get; set; }
        [InverseProperty("IdAlumnoNavigation")]
        [JsonIgnore]
        public virtual ICollection<MatriculaAlumno> MatriculaAlumnos { get; set; }
    }
}
