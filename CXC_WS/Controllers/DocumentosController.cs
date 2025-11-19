using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
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
  //  [Authorize]

    public class DocumentosController : ControllerBase
    {
        private readonly cxc_newContext _context;

        public DocumentosController(cxc_newContext context)
        {
            _context = context;
        }

        // GET: api/Documento
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Documento>>> GetDocumentos()
        {
          if (_context.Documentos == null)
          {
              return NotFound();
          }
            return await _context.Documentos.Include(x=>x.Conceptos).Include(y=>y.Pagos).ToListAsync();
        }
        
        [HttpGet]
        [Route("/api/Documentos/GetDocumentosByEstado/{id}")]
        public async Task<ActionResult<IEnumerable<Documento>>> GetDocumentosByEstado(int id)
        {
            if (_context.Documentos == null)
            {
                return NotFound();
            }
            var Imagen = await _context.Documentos.Where(x=>x.Estado == id && x.FechaSaldo >= DateTime.Today.AddDays(-30)).ToListAsync();

            if (Imagen == null)
            {
                return NotFound();
            }

            return Imagen;
        }

        // GET: api/Documento/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VwDocumentoCliente>> GetDocumento(int id)
        {
          if (_context.VwDocumentoClientes == null)
          {
              return NotFound();
          }
            var Documento = await _context.VwDocumentoClientes.Where(x=>x.Id == id).FirstOrDefaultAsync();

            if (Documento == null)
            {
                return NotFound();
            }

            return Documento;
        }

        // PUT: api/Documento/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
       /* [HttpPut("{id}")]
        public async Task<IActionResult> PutDocumento(int id, Documento Documento)
        {
            if (id != Documento.Id)
            {
                return BadRequest();
            }

            _context.Entry(Documento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/

        // POST: api/Documento
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      /*  [HttpPost]
        public async Task<ActionResult<List<Documento>>> PostDocumentos(List<Documento> documentos)
        {
            if (_context.Documentos == null)
            {
                return Problem("Entity set 'cxcContext.Documentos' is null.");
            }

            foreach (var documento in documentos)
            {
                _context.Documentos.Add(documento);
            }

            await _context.SaveChangesAsync();

            // Return the created documents with their assigned IDs
            return CreatedAtAction("GetDocumento", documentos);
        }*/
      
      [HttpPost]
      public async Task<ActionResult<Documento>> PostDocumento(Documento documento)
      {
          if (_context.Documentos == null)
          {
              return Problem("Entity set 'cxcContext.Documentos' is null.");
          }
          
          // Add the main Documento to the Documentos table
          documento.Estado = 3;
          documento.FechaCreacion = DateTime.Now;
          _context.Documentos.Add(documento);
          //await _context.SaveChangesAsync();
          var lastDocID = _context.Documentos.Max(x => x.Id);

          // Add each Concepto to the Conceptos table with the associated DocumentoId
          foreach (var concepto in documento.Conceptos)
          {
              concepto.IdDocDetalle = lastDocID;
              _context.Conceptos.Add(concepto);

          }
          
          HttpContext.Request.Headers.TryGetValue("User", out var headerValue);
          string usuario = headerValue.ToString();
          Logs logOperacione = new Logs();
          logOperacione.Descripcion = "Se creo el Documento: " +documento.Id ;
          logOperacione.Usuario = usuario;
          logOperacione.Fecha = DateTime.Now;
          _context.AddAsync(logOperacione); 

          
          await _context.SaveChangesAsync();
          return documento;
      }
      
      [HttpPut("{id}")]
      public async Task<IActionResult> PutDocumento(int id, Documento documento)
      {
          if (id != documento.Id)
          {
              return BadRequest("Mismatched ID in the URL and the request body.");
          }

          var existingDocumento = await _context.Documentos
              .Include(d => d.Conceptos)  // Include Conceptos to avoid breaking the relationship
              .FirstOrDefaultAsync(d => d.Id == id);

          if (existingDocumento == null)
          {
              return NotFound($"Documento with ID {id} not found.");
          }

          // Update scalar properties
          existingDocumento.TipoDoc = documento.TipoDoc;
          existingDocumento.FechaDoc = documento.FechaDoc;
          existingDocumento.IdCliente = documento.IdCliente;
          existingDocumento.Saldado = documento.Saldado;
          existingDocumento.Condiciones = documento.Condiciones;
          existingDocumento.Comentario = documento.Comentario;
          existingDocumento.Monto_Pagar = documento.Monto_Pagar;
          existingDocumento.Monto_Pendiente = documento.Monto_Pendiente;
          existingDocumento.FechaCreacion = documento.FechaCreacion;
          existingDocumento.Estado = documento.Estado;
          existingDocumento.Moneda = documento.Moneda;

          // Update or add Conceptos
          if (documento.Conceptos != null && documento.Conceptos.Any())
          {
              // Clear existing Conceptos and add the new ones
              existingDocumento.Conceptos.Clear();
              foreach (var concepto in documento.Conceptos)
              {
                  existingDocumento.Conceptos.Add(concepto);
              }
          }

          HttpContext.Request.Headers.TryGetValue("User", out var headerValue);
          string usuario = headerValue.ToString();
          Logs logOperacione = new Logs();
          logOperacione.Descripcion = "Se modifico el Documento: " +documento.Id ;
          logOperacione.Usuario = usuario;
          logOperacione.Fecha = DateTime.Now;
          _context.AddAsync(logOperacione); 

          
          try
          {
              await _context.SaveChangesAsync();
          }
          catch (DbUpdateConcurrencyException)
          {
              if (!DocumentoExists(id))
              {
                  return NotFound($"Documento with ID {id} not found.");
              }
              else
              {
                  throw;  // Handle other concurrency issues if needed
              }
          }

          return NoContent();  // Successful update
      }


        // DELETE: api/Documento/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocumento(int id)
        {
            if (_context.Documentos == null)
            {
                return NotFound();
            }
            var Documento = await _context.Documentos.FindAsync(id);
            if (Documento == null)
            {
                return NotFound();
            }

            HttpContext.Request.Headers.TryGetValue("User", out var headerValue);
            string usuario = headerValue.ToString();
            Logs logOperacione = new Logs();
            logOperacione.Descripcion = "Se elimino el Documento: " +id ;
            logOperacione.Usuario = usuario;
            logOperacione.Fecha = DateTime.Now;
            _context.AddAsync(logOperacione); 

            
            _context.Documentos.Remove(Documento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocumentoExists(int id)
        {
            return (_context.Documentos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        
        [HttpGet]
        [Route("/api/Documentos/GetLastIdDoc/")]
        public async Task<int> GetLastIdDoc()
        {
            var a =   _context.Documentos.Select(x=>x.Id).Max();

            return  a;
        }
        
       /* [HttpPost]
        [Route("/api/Documentos/Saldar/")]
        public async Task<ActionResult<Pago>> Saldar(Pago pago)
        {
            if (_context.Estados == null)
            {
                return Problem("Entity set 'cxcContext.Estados'  is null.");
            }

            var valorTotal = _context.Documentos.Where(x => x.Id == pago.IdDocumento).FirstOrDefault();
            await  _context.Pagos.AddAsync(pago);
            
            if (pago.Monto >= valorTotal.Monto_Pendiente )
            {
                try
                {
                    await _context.Documentos.FromSqlRaw("UPDATE DOCUMENTOS SET SALDADO = TRUE, estado = 6 WHERE ID =" + pago.IdDocumento)
                        .ToListAsync();
                }
                catch
                {
                }
            }
            else
            {
                try
                {
                    await   _context.Documentos.FromSqlRaw("UPDATE DOCUMENTOS SET estado = 5 WHERE ID =" + pago.IdDocumento)
                        .ToListAsync();
                }
                catch
                {
                }
            }
            
            await _context.SaveChangesAsync();

            return pago;
        }*/
       
       [HttpPost]
       [Route("/api/Documentos/Saldar/")]
       public async Task<ActionResult<IEnumerable<Pago>>> Saldar([FromBody] IEnumerable<Pago> pagos)
       {
           if (_context.Estados == null)
           {
               return Problem("Entity set 'cxcContext.Estados'  is null.");
           }

           foreach (var pago in pagos)
           {

               var valorTotal = _context.Documentos.Where(x => x.Id == pago.DocumentoId).FirstOrDefault();
               pago.FechaPago = DateTime.Now;
               var fechaSaldo = DateTime.Now;
               await _context.Pagos.AddAsync(pago);

               if (pago.Monto >= valorTotal.Monto_Pendiente)
               {
                   try
                   {
                       /*await _context.Documentos
                           .FromSqlRaw("UPDATE DOCUMENTOS SET SALDADO = 1, estado = 6, Monto_Pendiente = 0, FechaSaldo = "+fechaSaldo+" WHERE ID =" + pago.DocumentoId)
                           .ToListAsync();*/
                       
                       await _context.Database.ExecuteSqlInterpolatedAsync($@"
    UPDATE DOCUMENTOS 
    SET SALDADO = 1, 
        estado = 6, 
        Monto_Pendiente = 0, 
        FechaSaldo = {fechaSaldo:yyyy-MM-dd HH:mm:ss}
    WHERE ID = {pago.DocumentoId}");
                   }
                   catch
                   {
                   }
               }
               else
               {
                   try
                   {
                       var montoPendiente = valorTotal.Monto_Pendiente - pago.Monto;
                       await _context.Documentos
                           .FromSqlRaw("UPDATE DOCUMENTOS SET estado = 5, Monto_Pendiente ="+ montoPendiente +" WHERE ID =" + pago.DocumentoId)
                           .ToListAsync();
                   }
                   catch
                   {
                   }
               }
           }
           
           await _context.SaveChangesAsync();

           return Ok(pagos);
       }
    }
}
