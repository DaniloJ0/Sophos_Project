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
    public class ProfesoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProfesoresController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Profesores
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Profesor>>>  GetProfesors()
        //{
        //    return await _context.Profesors.ToListAsync();
        //}


        [HttpGet]
        public IActionResult Profesores()
        {
            List<Profesor> profesores = new List<Profesor>();
            try
            {
                profesores = _context.Profesors.ToList();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = profesores });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, response = profesores });
            }
        }

        [HttpGet]
        [Route("Cursos")]
        public IActionResult ProfesoresCursos()
        {
            List<Curso> cursos = new List<Curso>();
            try
            {
                cursos = _context.Cursos.Where(x => x.IdProfesor == 2).ToList();
                return StatusCode(StatusCodes.Status200OK, cursos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, response = cursos });
            }
        }

        // GET: api/Profesores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Profesor>> GetProfesor(int id)
        {
            var profesor = await _context.Profesors.FindAsync(id);

            if (profesor == null)
            {
                return NotFound();
            }

            return profesor;
        }

        // PUT: api/Profesores/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfesor(int id, Profesor profesor)
        {
            if (id != profesor.Id)
            {
                return BadRequest("Profesor no encontrado");
            }

            _context.Entry(profesor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfesorExists(id))
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


        // POST: api/Profesores
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Profesors>> PostProfesor(Profesors profesor)
        //{
        //    _context.Profesors.Add(profesor);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetProfesor", new { id = profesor.Id }, profesor);
        //}

        [HttpPost]
        public IActionResult Guardar([FromBody] Profesor prof)
        {
            try
            {
                _context.Profesors.Add(prof);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status201Created, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }




        // DELETE: api/Profesores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfesor(int id)
        {
            var profesor = await _context.Profesors.FindAsync(id);
            if (profesor == null)
            {
                return NotFound();
            }
            try
            {
                _context.Profesors.Remove(profesor);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, new { mensaje = ex.Message });
            }


        }

        private bool ProfesorExists(int id)
        {
            return _context.Profesors.Any(e => e.Id == id);
        }
    }
}
