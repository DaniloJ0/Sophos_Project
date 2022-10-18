﻿using System;
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
        public async Task<ActionResult<IEnumerable<CursosRealizado>>> GetCursosRealizados()
        {
            return await _context.CursosRealizados.ToListAsync();
        }

        // GET: api/CursosRealizados/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CursosRealizado>> GetCursosRealizado(int id)
        {
            var cursosRealizado = await _context.CursosRealizados.FindAsync(id);

            if (cursosRealizado == null)
            {
                return NotFound();
            }

            return cursosRealizado;
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
            _context.CursosRealizados.Add(cursosRealizado);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCursosRealizado", new { id = cursosRealizado.Id }, cursosRealizado);
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

            _context.CursosRealizados.Remove(cursosRealizado);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CursosRealizadoExists(int id)
        {
            return _context.CursosRealizados.Any(e => e.Id == id);
        }
    }
}