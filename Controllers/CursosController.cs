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
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null) return NotFound();
            try
            {
                int numeroEstudiantes = await _context.MatriculaAlumnos.Where(x => x.IdCurso == id).CountAsync();
                var profesorCurso = await _context.Profesors.FindAsync(curso.IdProfesor);
                var profesor = profesorCurso is null || profesorCurso.Nombre.Length < 2 ? "Pendiente" : profesorCurso.Nombre + " " + profesorCurso.Apellido;
                var matriculas = await _context.MatriculaAlumnos.ToListAsync();
                var allAlumnos = await _context.Alumnos.ToListAsync();
                // Join Matricula x Alumnos para obtener los alumnos de un curso
                var alumnos = from alumnoM in matriculas
                              join alumno in allAlumnos on alumnoM.IdAlumno equals alumno.Id
                              select alumno;
                var cursoPreRequisito = await _context.Cursos.FindAsync(curso.IdCursoPre);
                var preRequisito = cursoPreRequisito is null || cursoPreRequisito.Nombre.Length < 2 ? "No tiene pre-requisito" : cursoPreRequisito.Nombre;
                var data = new { curso.Id, curso.Nombre, numeroEstudiantes, profesor, curso.Creditos, curso.Cupos, preRequisito, alumnos };
                return StatusCode(StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, new { mensaje = ex.Message });
            }
        }

        // PUT: api/Cursos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurso(int id, Curso curso)
        {
            if (id != curso.Id) return BadRequest();

            var profesor = await _context.Profesors.FindAsync(curso.IdProfesor);
            if (profesor == null) return BadRequest("El profesor no existe");

            var periodo = await _context.Periodos.FindAsync(curso.IdPeriodo);
            if (periodo == null) return BadRequest("El Periodo no existe");

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

            return NoContent();
        }

        // POST: api/Cursos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Curso>> PostCurso(Curso curso)
        {
            var profesor = await _context.Profesors.FindAsync(curso.IdProfesor);
            if (profesor == null) return BadRequest("El profesor no existe");

            var periodo = await _context.Periodos.FindAsync(curso.IdPeriodo);
            if (periodo == null) return BadRequest("El Periodo no existe");

            if (curso.IdCursoPre != null)
            {
                var cursoPre = await _context.Cursos.FindAsync(curso.IdCursoPre);
                if (cursoPre == null) return BadRequest("El Curso no existe no existe");
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

                return NoContent();
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
