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
    [Authorize]
    public class TipoPagoController : ControllerBase
    {
        private readonly cxc_newContext _context;

        public TipoPagoController(cxc_newContext context)
        {
            _context = context;
        }

        // GET: api/TipoPago
        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<TipoPago>>> GetTipoPagos()
        {
          if (_context.TipoPagos == null)
          {
              return NotFound();
          }
            return await _context.TipoPagos.ToListAsync();
        }

        // GET: api/TipoPago/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoPago>> GetTipoPago(int id)
        {
          if (_context.TipoPagos == null)
          {
              return NotFound();
          }
            var TipoPago = await _context.TipoPagos.FindAsync(id);

            if (TipoPago == null)
            {
                return NotFound();
            }

            return TipoPago;
        }

        // PUT: api/TipoPago/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoPago(int id, TipoPago TipoPago)
        {
            if (id != TipoPago.Id)
            {
                return BadRequest();
            }

            _context.Entry(TipoPago).State = EntityState.Modified;

            HttpContext.Request.Headers.TryGetValue("User", out var headerValue);
            string usuario = headerValue.ToString();
            Logs logOperacione = new Logs();
            logOperacione.Descripcion = "Se modifico el tipoPago: " +TipoPago.Id ;
            logOperacione.Usuario = usuario;
            logOperacione.Fecha = DateTime.Now;
            _context.AddAsync(logOperacione); 
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoPagoExists(id))
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

        // POST: api/TipoPago
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TipoPago>> PostTipoPago(TipoPago TipoPago)
        {
          if (_context.TipoPagos == null)
          {
              return Problem("Entity set 'cxcContext.TipoPagos'  is null.");
          }
            _context.TipoPagos.Add(TipoPago);
            
            HttpContext.Request.Headers.TryGetValue("User", out var headerValue);
            string usuario = headerValue.ToString();
            Logs logOperacione = new Logs();
            logOperacione.Descripcion = "Se creo el tipoPago: " +TipoPago.Descripcion ;
            logOperacione.Usuario = usuario;
            logOperacione.Fecha = DateTime.Now;
            _context.AddAsync(logOperacione); 
            
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoPago", new { id = TipoPago.Id }, TipoPago);
        }

        // DELETE: api/TipoPago/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoPago(int id)
        {
            if (_context.TipoPagos == null)
            {
                return NotFound();
            }
            var TipoPago = await _context.TipoPagos.FindAsync(id);
            if (TipoPago == null)
            {
                return NotFound();
            }

            _context.TipoPagos.Remove(TipoPago);
            
            HttpContext.Request.Headers.TryGetValue("User", out var headerValue);
            string usuario = headerValue.ToString();
            Logs logOperacione = new Logs();
            logOperacione.Descripcion = "Se elimino el tipoPago: " +id ;
            logOperacione.Usuario = usuario;
            logOperacione.Fecha = DateTime.Now;
            _context.AddAsync(logOperacione); 
            
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipoPagoExists(int id)
        {
            return (_context.TipoPagos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
