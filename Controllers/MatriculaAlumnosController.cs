using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.DBContext;
using backend.Models;
using System.Linq.Expressions;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatriculaAlumnosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MatriculaAlumnosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/MatriculaAlumnos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatriculaAlumno>>> GetMatriculaAlumnos()
        {
            try
            {
                return await _context.MatriculaAlumnos.ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        // GET: api/MatriculaAlumnos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MatriculaAlumno>> GetMatriculaAlumno(int id)
        {
            try
            {
                var matriculaAlumno = await _context.MatriculaAlumnos.FindAsync(id);
                if (matriculaAlumno == null) return NotFound();
                return matriculaAlumno;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }

        }

        // PUT: api/MatriculaAlumnos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMatriculaAlumno(int id, MatriculaAlumno matriculaAlumno)
        {
            if (id != matriculaAlumno.Id) return BadRequest();
            _context.Entry(matriculaAlumno).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MatriculaAlumnoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/MatriculaAlumnos
        [HttpPost]
        public async Task<ActionResult<MatriculaAlumno>> PostMatriculaAlumno(MatriculaAlumno matriculaAlumno)
        {
            var alumno = await _context.Alumnos.FindAsync(matriculaAlumno.IdAlumno);
            if (alumno == null) return NotFound("El alumno no existe");

            var curso = await _context.Cursos.FindAsync(matriculaAlumno.IdCurso);
            if (curso == null) return NotFound("El Curso no existe");

            //Verificacion si se encuentra matriculado
            var verificacionMatricula = _context.MatriculaAlumnos.Where(x => x.IdCurso == matriculaAlumno.IdCurso && x.IdAlumno == matriculaAlumno.IdAlumno);
            if (verificacionMatricula == null) return Conflict("El alumno ya se encuentra matriculado");

            //Verificacion de Pre-Requisito de curso
            if (curso.IdCursoPre != null)
            {
                var validarPreCurso = _context.CursosRealizados.Where(x => x.IdCurso == matriculaAlumno.IdCurso && x.IdAlumno == matriculaAlumno.IdAlumno);
                if (validarPreCurso == null) return Conflict("El alumno no ha realizado el curso pre-requisito de " + curso.Nombre);
            }

            _context.MatriculaAlumnos.Add(matriculaAlumno);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MatriculaAlumnoExists(matriculaAlumno.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(StatusCodes.Status201Created);
        }

        // DELETE: api/MatriculaAlumnos/Completado/5
        [HttpDelete("Completado/{id}")]
        public async Task<IActionResult> DeleteMatriculaAlumnoCompletada(int id)
        {
            var matriculaAlumno = await _context.MatriculaAlumnos.FindAsync(id);
            if (matriculaAlumno == null) return NotFound();
            try
            {
                // Add Course Done in CursosRealizados
                CursosRealizado cursosRealizado = new()
                {
                    IdCurso = matriculaAlumno.IdCurso,
                    IdAlumno = matriculaAlumno.IdAlumno
                };

                _context.CursosRealizados.Add(cursosRealizado);
                _context.MatriculaAlumnos.Remove(matriculaAlumno);

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }

        }

        // DELETE: api/MatriculaAlumnos/Realizado/5
        [HttpDelete("Realizado/{id}")]
        public async Task<IActionResult> DeleteMatriculaAlumnoRetirada(int id)
        {
            var matriculaAlumno = await _context.MatriculaAlumnos.FindAsync(id);
            if (matriculaAlumno == null) return NotFound();
            try
            {
                _context.MatriculaAlumnos.Remove(matriculaAlumno);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        private bool MatriculaAlumnoExists(int id)
        {
            return _context.MatriculaAlumnos.Any(e => e.Id == id);
        }
    }
}
