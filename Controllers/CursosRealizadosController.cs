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
    public class CursosRealizadosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CursosRealizadosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/CursosRealizados
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CursosRealizado>>> CursosRealizados()
        {
            try
            {
                var cursosRealizado = await _context.CursosRealizados.ToListAsync();
                return Ok(cursosRealizado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, new { mensaje = ex.Message });
            }
        }

        // GET: api/CursosRealizados/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CursosRealizado>> GetCursosRealizado(int id)
        {
            try
            {
                var cursosRealizado = await _context.CursosRealizados
                    .Where(x => x.Id == id)
                    .Include(x => x.IdCursoNavigation)
                    .ToListAsync();

                if (cursosRealizado == null || cursosRealizado.Count == 0) return NotFound("El id no existe");

                return Ok(cursosRealizado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }
        // GET: api/CursosRealizados/Alumno/5
        [HttpGet]
        [Route("Alumno/{id:int}")]
        public async Task<ActionResult<CursosRealizado>> GetCursosRealizadoAlumno(int id)
        {
            List<CursosRealizado> cursosRealizado = new();
            try
            {
                cursosRealizado = await _context.CursosRealizados
                    .Where(x => x.IdAlumno == id)
                    .Include(x => x.IdCursoNavigation)
                    .ToListAsync();

                if (cursosRealizado == null || cursosRealizado.Count == 0) return NotFound("No hay datos de ese alumno");

                return Ok(cursosRealizado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, new { mensaje = ex.Message });
            }


        }

        // PUT: api/CursosRealizados/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCursosRealizado(int id, CursosRealizado cursosRealizado)
        {
            if (id != cursosRealizado.Id)
            {
                return BadRequest();
            }

            _context.Entry(cursosRealizado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CursosRealizadoExists(id))
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

        // POST: api/CursosRealizados
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CursosRealizado>> PostCursosRealizado(CursosRealizado cursosRealizado)
        {
            var alumno = await _context.Alumnos.FindAsync(cursosRealizado.IdAlumno);
            if (alumno == null) return NotFound("El alumno no existe");

            var curso = await _context.Cursos.FindAsync(cursosRealizado.IdAlumno);
            if (curso == null) return NotFound("El Curso no existe");

            try
            {
                _context.CursosRealizados.Add(cursosRealizado);
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, new { mensaje = ex.Message });
            }
        }

        // DELETE: api/CursosRealizados/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCursosRealizado(int id)
        {
            var cursosRealizado = await _context.CursosRealizados.FindAsync(id);
            if (cursosRealizado == null)
            {
                return NotFound();
            }
            try
            {
                _context.CursosRealizados.Remove(cursosRealizado);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, new { mensaje = ex.Message });
            }
        }
        private bool CursosRealizadoExists(int id)
        {
            return _context.CursosRealizados.Any(e => e.Id == id);
        }
    }
}
