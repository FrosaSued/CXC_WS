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

    public class ConceptosController : ControllerBase
    {
        private readonly cxc_newContext _context;

        public ConceptosController(cxc_newContext context)
        {
            _context = context;
        }

        // GET: api/Cliente
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Concepto>>> GetConcepto()
        {
          if (_context.Conceptos == null)
          {
              return NotFound();
          }
            return await _context.Conceptos.ToListAsync();
        }

        // GET: api/Cliente/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Concepto>> GetConcepto(int id)
        {
          if (_context.Conceptos == null)
          {
              return NotFound();
          }
            var cliente = await _context.Conceptos.FindAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        // PUT: api/Cliente/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConcepto(int id, Concepto cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest();
            }

            _context.Entry(cliente).State = EntityState.Modified;

            
            HttpContext.Request.Headers.TryGetValue("User", out var headerValue);
            string usuario = headerValue.ToString();
            Logs logOperacione = new Logs();
            logOperacione.Descripcion = "Se modifico el concepto: " +cliente.Id ;
            logOperacione.Usuario = usuario;
            logOperacione.Fecha = DateTime.Now;
            _context.AddAsync(logOperacione); 
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConceptoExists(id))
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

        // POST: api/Cliente
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Concepto>> PostConcepto(Concepto cliente)
        {
          if (_context.Conceptos == null)
          {
              return Problem("Entity set 'cxcContext.Clientes'  is null.");
          }

          cliente.Estado = 1;
            _context.Conceptos.Add(cliente);
            
            HttpContext.Request.Headers.TryGetValue("User", out var headerValue);
            string usuario = headerValue.ToString();
            Logs logOperacione = new Logs();
            logOperacione.Descripcion = "Se creo el concepto: " +cliente.Concepto1 ;
            logOperacione.Usuario = usuario;
            logOperacione.Fecha = DateTime.Now;
            _context.AddAsync(logOperacione); 
            
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConcepto", new { id = cliente.Id }, cliente);
        }

        // DELETE: api/Cliente/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConcepto(int id)
        {
            if (_context.Conceptos == null)
            {
                return NotFound();
            }
            var cliente = await _context.Conceptos.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            _context.Conceptos.Remove(cliente);
            
            HttpContext.Request.Headers.TryGetValue("User", out var headerValue);
            string usuario = headerValue.ToString();
            Logs logOperacione = new Logs();
            logOperacione.Descripcion = "Se elimino el concepto: " +cliente.Concepto1 ;
            logOperacione.Usuario = usuario;
            logOperacione.Fecha = DateTime.Now;
            _context.AddAsync(logOperacione); 
            
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConceptoExists(int id)
        {
            return (_context.Conceptos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
