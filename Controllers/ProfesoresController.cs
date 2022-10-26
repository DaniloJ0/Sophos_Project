using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.DBContext;
using backend.Models;
using Microsoft.CodeAnalysis.Operations;
using Newtonsoft.Json.Linq;

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

        //GET: api/Profesores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Profesor>>> GetProfesors()
        {
            try
            {
                return await _context.Profesors
                    .Include(x => x.IdDeptNavigation)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


        // GET: api/Profesores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Profesor>> GetProfesor(int id)
        {
            try
            {
                var profesor = await _context.Profesors
                    .Where(x => x.Id == id)
                    .Include(x => x.IdDeptNavigation)
                    .Include(x => x.Cursos)
                    .FirstOrDefaultAsync();

                if (profesor == null) return NotFound("El profesor no existe");
                return StatusCode(StatusCodes.Status200OK, profesor);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }

        }


        // PUT: api/Profesores/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfesor(int id, Profesor profesor)
        {
            if (id != profesor.Id) return BadRequest("Id no concuerda con el parametro");
            
            //Valida que no ingrese una facultad que no exista
            var facultad = await _context.Facultads.FindAsync(profesor.IdDept);
            if (facultad == null) return NotFound("El Id de la facultad no existe");

            _context.Entry(profesor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfesorExists(id))
                {
                    return NotFound("Profesor no encontrado");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        //POST: api/Profesores
        [HttpPost]
        public async Task<ActionResult<Profesor>> PostProfesor(Profesor profesor)
        {
            var facultad = await _context.Facultads.FindAsync(profesor.IdDept);
            if (facultad == null) return NotFound("El Id de la facultad no existe");
            try
            {
                _context.Profesors.Add(profesor);
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, new { mensaje = ex.Message });
            }
        }

        // DELETE: api/Profesores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfesor(int id)
        {
            var profesor = await _context.Profesors.FindAsync(id);
            if (profesor == null) return NotFound();
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
