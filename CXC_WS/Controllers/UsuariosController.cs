using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Newtonsoft.Json;
using CXC_WS.Services;
using CXC_WS.Interfaces;
using Microsoft.AspNetCore.Authorization;
using CXC_WS.Data.Models;

namespace CXC_WS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuario _usuarioService;
        private readonly cxc_newContext _context;
        public IConfiguration _configuration;
        // public UsuariosController(IUsuario usuarios) => _usuarioService = usuarios;
        public UsuariosController(cxc_newContext context, IUsuario usuarios, IConfiguration configuration)
        {
            _context = context;
            _usuarioService = usuarios;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Register/")]
       // [Authorize]
        public ActionResult Register([FromBody] Usuario usuario)
        {
            var insertemp = _usuarioService.Register(usuario);
            return Ok(insertemp);
        }

        [HttpPost]
        [Route("GetUser/")]

        public async Task<IActionResult> GetUser([FromBody] Usuario usuario)
        {
            var user = await _usuarioService.GetUser(usuario);

            if (user == null)
            {
                return BadRequest(new { message = "Credenciales invalidas." });

            }
            if (user.Usuario1 == usuario.Usuario1)
            {
                var jwt = _configuration.GetSection("Jwt").Get<Jwt>();
                var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Usuario1", user.Usuario1.ToString()),
                    // new Claim("userName", user.UserName),
                    new Claim("Password", user.Password),
                    // new Claim("salt", user.Salt)
                };

                //var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.key));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    // issuer: "http://localhost:5000",
                    jwt.Issuer,
                    // audience: "http://localhost:5000",
                    jwt.Audience,
                    claims,
                    expires: DateTime.Now.AddMinutes(250),
                    signingCredentials: signinCredentials
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                var resultado = new { Usuario = user, Token = tokenString };
                return Ok(resultado);
                
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetAllUsuarios()
        {
            var items = await _usuarioService.GetAllUsuarios();
            return items;
        }

     /*   [HttpPut]
        [Route("PutUsuario/{id}")]
        //[Authorize]
       // public async Task<ActionResult<Usuario>> PutUsuario([FromRoute] int id, [FromBody] Usuario usuario)
        public async Task<ActionResult<Usuario>> PutUsuario([FromRoute] int id, [FromBody] Usuario usuario)
        {
         //   var checkFirstLogin =  await _context.Usuarios.Where(x => x.Usuario1 == usuario.Usuario1 && x.Estado == 3).CountAsync();
            
            var query =  await (from a in _context.Usuarios
                where a.Usuario1 == usuario.Usuario1
                select a).FirstAsync();
            //if (checkFirstLogin > 0)
            //{
            var validarpassword =  _usuarioService.ValidarPassword(usuario.Password, query.Password2, query.Salt);
            
            await _usuarioService.PutUsuario(id, usuario);
                
                
                if (validarpassword)
                {
                    var query2 = await (from a in _context.Usuarios
                        where a.Usuario1 == usuario.Usuario1
                        select a).FirstAsync();
                    //return "Password cambiada de manera exitosa.";
                    //return Ok("Password cambiada de manera exitosa.");
                    return Ok(query2);

                }
                else
                {                   // return Ok(query2);

                    return Ok( "false" );
                    //return false;
                    

                }
        }*/
        
        [HttpGet]
        [Route("IsFirstLogin/{Usuario1}")]
        public async Task<bool> IsFirstLogin(string Usuario1)
        {
            var items = await _context.Usuarios.Where(x => x.Usuario1 == Usuario1).FirstOrDefaultAsync();
            if (items == null)
            {
                return false;
            }
            
            if (items.Estado == 3)
            {
                items.Estado = 1;
                _context.SaveChangesAsync();
                return true;
            }
            
            else
            {
                return false;
            }

        }
        
        [HttpGet]
        [Route("TestApi/")]
        [Authorize]
        public async Task<bool> TestApi()
        {
            return true;
        }

                [HttpGet]
        [Route("GetRol/{correo}")]
        public async Task<Usuario> GetRol(string correo)
        {
            var items = await _context.Usuarios.Where(x => x.Correo == correo).FirstOrDefaultAsync();
            return items;

        }

    }
}
