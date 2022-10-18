﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public virtual Alumno IdAlumnoNavigation { get; set; } = null!;
    }
}