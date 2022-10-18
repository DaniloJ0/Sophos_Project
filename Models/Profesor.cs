using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    [Table("profesor")]
    public partial class Profesor
    {
        public Profesor()
        {
            Cursos = new HashSet<Curso>();
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
        [Column("email")]
        [StringLength(50)]
        [Unicode(false)]
        public string Email { get; set; } = null!;
        [Column("max_titulo")]
        [StringLength(70)]
        [Unicode(false)]
        public string MaxTitulo { get; set; } = null!;
        [Column("exp_year")]
        public int ExpYear { get; set; }
        [Column("id_dept")]
        public int IdDept { get; set; }

        [ForeignKey("IdDept")]
        [InverseProperty("Profesors")]
        [JsonIgnore]
        public virtual Facultad? IdDeptNavigation { get; set; } // = null!;
        [InverseProperty("IdProfesorNavigation")]
        [JsonIgnore]
        public virtual ICollection<Curso> Cursos { get; set; }
    }
}
