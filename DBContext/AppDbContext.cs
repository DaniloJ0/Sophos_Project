using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using backend.Models;

namespace backend.DBContext
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Alumno> Alumnos { get; set; } = null!;
        public virtual DbSet<Curso> Cursos { get; set; } = null!;
        public virtual DbSet<CursosRealizado> CursosRealizados { get; set; } = null!;
        public virtual DbSet<Facultad> Facultads { get; set; } = null!;
        public virtual DbSet<MatriculaAlumno> MatriculaAlumnos { get; set; } = null!;
        public virtual DbSet<Periodo> Periodos { get; set; } = null!;
        public virtual DbSet<Profesor> Profesors { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Alumno>(entity =>
            {
                entity.HasOne(d => d.IdDeptNavigation)
                    .WithMany(p => p.Alumnos)
                    .HasForeignKey(d => d.IdDept)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_alumno_facultad");
            });

            modelBuilder.Entity<Curso>(entity =>
            {
                entity.HasOne(d => d.IdPeriodoNavigation)
                    .WithMany(p => p.Cursos)
                    .HasForeignKey(d => d.IdPeriodo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_curso_periodo");

                entity.HasOne(d => d.IdProfesorNavigation)
                    .WithMany(p => p.Cursos)
                    .HasForeignKey(d => d.IdProfesor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_curso_profesor");
            });

            modelBuilder.Entity<CursosRealizado>(entity =>
            {
                entity.HasOne(d => d.IdAlumnoNavigation)
                    .WithMany(p => p.CursosRealizados)
                    .HasForeignKey(d => d.IdAlumno)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_cursos_realizados_alumno");
            });

            modelBuilder.Entity<MatriculaAlumno>(entity =>
            {
                //entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.IdCurso).ValueGeneratedOnAdd();

                entity.HasOne(d => d.IdAlumnoNavigation)
                    .WithMany(p => p.MatriculaAlumnos)
                    .HasForeignKey(d => d.IdAlumno)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_matricula_alumno_alumno");

                entity.HasOne(d => d.IdCursoNavigation)
                    .WithMany(p => p.MatriculaAlumnos)
                    .HasForeignKey(d => d.IdCurso)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_matricula_alumno_curso");
            });

            modelBuilder.Entity<Profesor>(entity =>
            {
                entity.HasOne(d => d.IdDeptNavigation)
                    .WithMany(p => p.Profesors)
                    .HasForeignKey(d => d.IdDept)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_profesor_facultad");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
