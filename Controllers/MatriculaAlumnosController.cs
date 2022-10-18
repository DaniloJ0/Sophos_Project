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
            return await _context.MatriculaAlumnos.ToListAsync();
        }

        // GET: api/MatriculaAlumnos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MatriculaAlumno>> GetMatriculaAlumno(int id)
        {
            var matriculaAlumno = await _context.MatriculaAlumnos.FindAsync(id);

            if (matriculaAlumno == null)
            {
                return NotFound();
            }

            return matriculaAlumno;
        }

        // PUT: api/MatriculaAlumnos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMatriculaAlumno(int id, MatriculaAlumno matriculaAlumno)
        {
            if (id != matriculaAlumno.Id)
            {
                return BadRequest();
            }

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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MatriculaAlumno>> PostMatriculaAlumno(MatriculaAlumno matriculaAlumno)
        {
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

            return CreatedAtAction("GetMatriculaAlumno", new { id = matriculaAlumno.Id }, matriculaAlumno);
        }

        // DELETE: api/MatriculaAlumnos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatriculaAlumno(int id)
        {
            var matriculaAlumno = await _context.MatriculaAlumnos.FindAsync(id);
            if (matriculaAlumno == null)
            {
                return NotFound();
            }

            _context.MatriculaAlumnos.Remove(matriculaAlumno);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MatriculaAlumnoExists(int id)
        {
            return _context.MatriculaAlumnos.Any(e => e.Id == id);
        }
    }
}
