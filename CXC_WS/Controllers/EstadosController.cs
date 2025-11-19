using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CXC_WS.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace CXC_WS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]

    public class EstadosController : ControllerBase
    {
        private readonly cxc_newContext _context;

        public EstadosController(cxc_newContext context)
        {
            _context = context;
        }

        // GET: api/estado
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estado>>> GetEstados()
        {
          if (_context.Estados == null)
          {
              return NotFound();
          }
            return await _context.Estados.ToListAsync();
        }

        // GET: api/estado/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Estado>> GetEstado(int id)
        {
          if (_context.Estados == null)
          {
              return NotFound();
          }
            var estado = await _context.Estados.FindAsync(id);

            if (estado == null)
            {
                return NotFound();
            }

            return estado;
        }

        // PUT: api/estado/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putestado(int id, Estado estado)
        {
            if (id != estado.Id)
            {
                return BadRequest();
            }

            _context.Entry(estado).State = EntityState.Modified;

            HttpContext.Request.Headers.TryGetValue("User", out var headerValue);
            string usuario = headerValue.ToString();
            Logs logOperacione = new Logs();
            logOperacione.Descripcion = "Se modifico el Estado: " +estado.Id ;
            logOperacione.Usuario = usuario;
            logOperacione.Fecha = DateTime.Now;
            _context.AddAsync(logOperacione); 
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadoExists(id))
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

        // POST: api/estado
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Estado>> PostEstado(Estado estado)
        {
          if (_context.Estados == null)
          {
              return Problem("Entity set 'cxcContext.Estados'  is null.");
          }

            _context.Estados.Add(estado);
            
            HttpContext.Request.Headers.TryGetValue("User", out var headerValue);
            string usuario = headerValue.ToString();
            Logs logOperacione = new Logs();
            logOperacione.Descripcion = "Se creo el estado: " +estado.Descripcion ;
            logOperacione.Usuario = usuario;
            logOperacione.Fecha = DateTime.Now;
            _context.AddAsync(logOperacione); 
            
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEstado", new { id = estado.Id }, estado);
        }

        // DELETE: api/estado/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstado(int id)
        {
            if (_context.Estados == null)
            {
                return NotFound();
            }
            var estado = await _context.Estados.FindAsync(id);
            if (estado == null)
            {
                return NotFound();
            }

            _context.Estados.Remove(estado);
            
            HttpContext.Request.Headers.TryGetValue("User", out var headerValue);
            string usuario = headerValue.ToString();
            Logs logOperacione = new Logs();
            logOperacione.Descripcion = "Se elimino el estado: " +estado.Id ;
            logOperacione.Usuario = usuario;
            logOperacione.Fecha = DateTime.Now;
            _context.AddAsync(logOperacione); 
            
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstadoExists(int id)
        {
            return (_context.Estados?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
