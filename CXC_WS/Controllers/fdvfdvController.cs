using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CXC_WS.Data.Models;

namespace CXC_WS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class fdvfdvController : ControllerBase
    {
        private readonly cxc_newContext _context;

        public fdvfdvController(cxc_newContext context)
        {
            _context = context;
        }

        // GET: api/fdvfdv
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alerta>>> GetAlertas()
        {
          if (_context.Alertas == null)
          {
              return NotFound();
          }
            return await _context.Alertas.ToListAsync();
        }

        // GET: api/fdvfdv/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Alerta>> GetAlerta(int id)
        {
          if (_context.Alertas == null)
          {
              return NotFound();
          }
            var alerta = await _context.Alertas.FindAsync(id);

            if (alerta == null)
            {
                return NotFound();
            }

            return alerta;
        }

        // PUT: api/fdvfdv/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlerta(int id, Alerta alerta)
        {
            if (id != alerta.Id)
            {
                return BadRequest();
            }

            _context.Entry(alerta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlertaExists(id))
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

        // POST: api/fdvfdv
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Alerta>> PostAlerta(Alerta alerta)
        {
          if (_context.Alertas == null)
          {
              return Problem("Entity set 'cxcContext.Alertas'  is null.");
          }
            _context.Alertas.Add(alerta);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAlerta", new { id = alerta.Id }, alerta);
        }

        // DELETE: api/fdvfdv/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlerta(int id)
        {
            if (_context.Alertas == null)
            {
                return NotFound();
            }
            var alerta = await _context.Alertas.FindAsync(id);
            if (alerta == null)
            {
                return NotFound();
            }

            _context.Alertas.Remove(alerta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AlertaExists(int id)
        {
            return (_context.Alertas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
