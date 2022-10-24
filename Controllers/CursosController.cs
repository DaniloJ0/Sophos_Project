using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.DBContext;
using backend.Models;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CursosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Cursos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Curso>>> GetCursos()
        {
            try
            {
                return await _context.Cursos.ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        // GET: api/Cursos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Curso>> GetCurso(int id)
        {
            try
            {
                var curso = await _context.Cursos
                .Where(x => x.Id == id)
                .Include(x => x.IdProfesorNavigation)
                .Include(x => x.MatriculaAlumnos)
                .Include(x => x.IdPeriodoNavigation)
                .ToListAsync();
                if (curso == null || curso.Count == 0) return NotFound("Curso no encontrado");

                var cursoPreRequisito = await _context.Cursos.FindAsync(curso.First().IdCursoPre);
                var preRequisito = cursoPreRequisito == null ? "No tiene pre-requisito" : cursoPreRequisito.Nombre;
                var data = new { curso, preRequisito };

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        // PUT: api/Cursos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurso(int id, Curso curso)
        {
            if (id != curso.Id) return BadRequest("El Id no concuerda con el parametro");

            var existCurso = await _context.Cursos.FindAsync(id);
            if (existCurso == null) return NotFound("Curso no encontrado");

            var profesor = await _context.Profesors.FindAsync(curso.IdProfesor);
            if (profesor == null) return NotFound("El profesor no existe");

            var periodo = await _context.Periodos.FindAsync(curso.IdPeriodo);
            if (periodo == null) return NotFound("El Periodo no existe");

            _context.Entry(curso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CursoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("El curso se ha actualizado");
        }

        // POST: api/Cursos
        [HttpPost]
        public async Task<ActionResult<Curso>> PostCurso(Curso curso)
        {
            var profesor = await _context.Profesors.FindAsync(curso.IdProfesor);
            if (profesor == null) return NotFound("El profesor no existe");

            var periodo = await _context.Periodos.FindAsync(curso.IdPeriodo);
            if (periodo == null) return NotFound("El Periodo no existe");

            if (curso.IdCursoPre != null)
            {
                var cursoPre = await _context.Cursos.FindAsync(curso.IdCursoPre);
                if (cursoPre == null) return NotFound("El Curso no existe");
            }
            try
            {
                _context.Cursos.Add(curso);
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, new { mensaje = ex.Message });
            }
        }

        // DELETE: api/Cursos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurso(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null) return NotFound();

            try
            {
                _context.Cursos.Remove(curso);
                await _context.SaveChangesAsync();

                return Ok("El curso se ha eliminado");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, new { mensaje = ex.Message });
            }
        }

        private bool CursoExists(int id)
        {
            return _context.Cursos.Any(e => e.Id == id);
        }
    }
}
