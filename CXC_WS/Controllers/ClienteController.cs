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

    public class ClienteController : ControllerBase
    {
        private readonly cxc_newContext _context;

        public ClienteController(cxc_newContext context)
        {
            _context = context;
        }

        // GET: api/Cliente
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
          if (_context.Clientes == null)
          {
              return NotFound();
          }
            return await _context.Clientes.ToListAsync();
        }

        // GET: api/Cliente/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
          if (_context.Clientes == null)
          {
              return NotFound();
          }
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        // PUT: api/Cliente/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente([FromRoute] int id, [FromBody] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                //return BadRequest();
            }

            var clienteActual = await _context.Clientes.Where(x => x.Id == id).FirstOrDefaultAsync();

            clienteActual.Condiciones = cliente.Condiciones;
            clienteActual.Estado = cliente.Estado;
            clienteActual.Ciudad = cliente.Ciudad;
            clienteActual.Moneda = cliente.Moneda;
            clienteActual.Direccion = cliente.Direccion;
            clienteActual.Nif = cliente.Nif;
            clienteActual.Nombre = cliente.Nombre;
            clienteActual.Pais = cliente.Pais;
            clienteActual.EstadoPais = cliente.EstadoPais;
            clienteActual.Telefono = cliente.Telefono;
            clienteActual.CuentaBanco = cliente.CuentaBanco;
            
            HttpContext.Request.Headers.TryGetValue("User", out var headerValue);
            string usuario = headerValue.ToString();
            Logs logOperacione = new Logs();
            logOperacione.Descripcion = "Se modifico el cliente: " +cliente.Nombre ;
            logOperacione.Usuario = usuario;
            logOperacione.Fecha = DateTime.Now;
            _context.AddAsync(logOperacione); 
            
            await _context.SaveChangesAsync();

            return Ok(cliente);
        }

        // POST: api/Cliente
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            var lastId = _context.Clientes.Select(x => x.Id).Max();
            cliente.Id = lastId + 1;
          cliente.Estado = 1;
           await _context.Clientes.AddAsync(cliente);
           
           HttpContext.Request.Headers.TryGetValue("User", out var headerValue);
           string usuario = headerValue.ToString();
           Logs logOperacione = new Logs();
           logOperacione.Descripcion = "Se Creo el cliente: " +cliente.Nombre ;
           logOperacione.Usuario = usuario;
           logOperacione.Fecha = DateTime.Now;
           _context.AddAsync(logOperacione); 
           
            await _context.SaveChangesAsync();

            return cliente;
        }

        // DELETE: api/Cliente/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            if (_context.Clientes == null)
            {
                return NotFound();
            }
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            _context.Clientes.Remove(cliente);
            
            HttpContext.Request.Headers.TryGetValue("User", out var headerValue);
            string usuario = headerValue.ToString();
            Logs logOperacione = new Logs();
            logOperacione.Descripcion = "Se elimino el cliente: " +cliente.Nombre ;
            logOperacione.Usuario = usuario;
            logOperacione.Fecha = DateTime.Now;
            _context.AddAsync(logOperacione); 
            
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClienteExists(int id)
        {
            return (_context.Clientes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
