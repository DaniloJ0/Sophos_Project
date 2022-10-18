using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    [Table("periodo")]
    public partial class Periodo
    {
        public Periodo()
        {
            Cursos = new HashSet<Curso>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("year")]
        [StringLength(4)]
        [Unicode(false)]
        public string Year { get; set; } = null!;
        [Column("semestre")]
        public int Semestre { get; set; }

        [InverseProperty("IdPeriodoNavigation")]
        public virtual ICollection<Curso> Cursos { get; set; }
    }
}
