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

    public class ConceptosDescripcionController : ControllerBase
    {
        private readonly cxc_newContext _context;

        public ConceptosDescripcionController(cxc_newContext context)
        {
            _context = context;
        }

        // GET: api/Cliente
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConceptosDescripcion>>> GetConceptosDescripcion()
        {
          if (_context.ConceptosDescripcions == null)
          {
              return NotFound();
          }
            return await _context.ConceptosDescripcions.ToListAsync();
        }

        // GET: api/Cliente/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ConceptosDescripcion>> GetConceptosDescripcion(int id)
        {
          if (_context.ConceptosDescripcions == null)
          {
              return NotFound();
          }
            var cliente = await _context.ConceptosDescripcions.FindAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        // PUT: api/Cliente/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConceptosDescripcion(int id, ConceptosDescripcion cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest();
            }

            _context.Entry(cliente).State = EntityState.Modified;

            HttpContext.Request.Headers.TryGetValue("User", out var headerValue);
            string usuario = headerValue.ToString();
            Logs logOperacione = new Logs();
            logOperacione.Descripcion = "Se modifico el conceptoDescripcion: " +cliente.Id ;
            logOperacione.Usuario = usuario;
            logOperacione.Fecha = DateTime.Now;
            _context.AddAsync(logOperacione); 
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConceptosDescripcionExists(id))
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
        public async Task<ActionResult<ConceptosDescripcion>> PostConceptosDescripcion(ConceptosDescripcion cliente)
        {

            _context.ConceptosDescripcions.Add(cliente);
            
            HttpContext.Request.Headers.TryGetValue("User", out var headerValue);
            string usuario = headerValue.ToString();
            Logs logOperacione = new Logs();
            logOperacione.Descripcion = "Se creo el conceptoDescripcion: " +cliente.Descripcion ;
            logOperacione.Usuario = usuario;
            logOperacione.Fecha = DateTime.Now;
            _context.AddAsync(logOperacione); 

            
            await _context.SaveChangesAsync();

            return cliente;

        }

        // DELETE: api/Cliente/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConceptosDescripcion(int id)
        {
            if (_context.ConceptosDescripcions == null)
            {
                return NotFound();
            }
            var cliente = await _context.ConceptosDescripcions.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            _context.ConceptosDescripcions.Remove(cliente);
            
            HttpContext.Request.Headers.TryGetValue("User", out var headerValue);
            string usuario = headerValue.ToString();
            Logs logOperacione = new Logs();
            logOperacione.Descripcion = "Se elimino el conceptoDescripcion: " +cliente.Descripcion ;
            logOperacione.Usuario = usuario;
            logOperacione.Fecha = DateTime.Now;
            _context.AddAsync(logOperacione); 

            
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConceptosDescripcionExists(int id)
        {
            return (_context.ConceptosDescripcions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
