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

    public class PagosController : ControllerBase
    {
        private readonly cxc_newContext _context;

        public PagosController(cxc_newContext context)
        {
            _context = context;
        }

        // GET: api/Pago
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pago>>> GetPagos()
        {
          if (_context.Pagos == null)
          {
              return NotFound();
          }
            return await _context.Pagos.ToListAsync();
        }

        // GET: api/Pago/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pago>> GetPago(int id)
        {
          if (_context.Pagos == null)
          {
              return NotFound();
          }
            var Pago = await _context.Pagos.FindAsync(id);

            if (Pago == null)
            {
                return NotFound();
            }

            return Pago;
        }

        // PUT: api/Pago/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPago(int id, Pago Pago)
        {
            if (id != Pago.Id)
            {
                return BadRequest();
            }

            _context.Entry(Pago).State = EntityState.Modified;

            HttpContext.Request.Headers.TryGetValue("User", out var headerValue);
            string usuario = headerValue.ToString();
            Logs logOperacione = new Logs();
            logOperacione.Descripcion = "Se modifico el pago: " +Pago.Id ;
            logOperacione.Usuario = usuario;
            logOperacione.Fecha = DateTime.Now;
            _context.AddAsync(logOperacione); 
            
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PagoExists(id))
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

        // POST: api/Pago
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pago>> PostPago(Pago Pago)
        {
          if (_context.Pagos == null)
          {
              return Problem("Entity set 'cxcContext.Pagos'  is null.");
          }
            _context.Pagos.Add(Pago);
            
            HttpContext.Request.Headers.TryGetValue("User", out var headerValue);
            string usuario = headerValue.ToString();
            Logs logOperacione = new Logs();
            logOperacione.Descripcion = "Se creo el pago: " +Pago.DocumentoId ;
            logOperacione.Usuario = usuario;
            logOperacione.Fecha = DateTime.Now;
            _context.AddAsync(logOperacione); 
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPago", new { id = Pago.Id }, Pago);
        }

        // DELETE: api/Pago/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePago(int id)
        {
            if (_context.Pagos == null)
            {
                return NotFound();
            }
            var Pago = await _context.Pagos.FindAsync(id);
            if (Pago == null)
            {
                return NotFound();
            }

            _context.Pagos.Remove(Pago);
            
            HttpContext.Request.Headers.TryGetValue("User", out var headerValue);
            string usuario = headerValue.ToString();
            Logs logOperacione = new Logs();
            logOperacione.Descripcion = "Se elimino el pago: " +Pago.Id ;
            logOperacione.Usuario = usuario;
            logOperacione.Fecha = DateTime.Now;
            _context.AddAsync(logOperacione); 
            
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PagoExists(int id)
        {
            return (_context.Pagos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
