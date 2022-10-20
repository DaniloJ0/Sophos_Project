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
    public class AlumnosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AlumnosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Alumnos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alumno>>> GetAlumnos()
        {
            try
            {
                return await _context.Alumnos.ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        // GET: api/Alumnos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Alumno>> GetAlumno(int id)
        {
            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno == null) return NotFound();

            try
            {
                var cursosMatriculados = await _context.MatriculaAlumnos.Where(x => x.IdAlumno == id).ToListAsync();
                var cursosRealizados = await _context.CursosRealizados.Where(x => x.IdAlumno == id).ToListAsync();
                var departamento = _context.Facultads.Where(x => x.Id == alumno.IdDept).First().Name;
                var datos = new { alumno.Nombre, alumno.Apellido, alumno.Semestre, alumno.CredtDisp, departamento, cursosMatriculados, cursosRealizados };
                return StatusCode(StatusCodes.Status200OK, datos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        // PUT: api/Alumnos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlumno(int id, Alumno alumno)
        {
            if (id != alumno.Id) return BadRequest();
            var facultad = await _context.Facultads.FindAsync(alumno.IdDept);
            if (facultad == null) return NotFound("El Id de la facultad no existe");

            _context.Entry(alumno).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlumnoExists(id))
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

        // POST: api/Alumnos
        [HttpPost]
        public async Task<ActionResult<Alumno>> PostAlumno(Alumno alumno)
        {
            var facultad = await _context.Facultads.FindAsync(alumno.IdDept);
            if (facultad == null) return NotFound("El Id de la facultad no existe");
            try
            {
                _context.Alumnos.Add(alumno);
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, new { mensaje = ex.Message });
            }
        }

        // DELETE: api/Alumnos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlumno(int id)
        {
            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno == null) return NotFound();
            try
            {
                _context.Alumnos.Remove(alumno);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, new { mensaje = ex.Message });
            }
        }

        private bool AlumnoExists(int id)
        {
            return _context.Alumnos.Any(e => e.Id == id);
        }
    }
}
