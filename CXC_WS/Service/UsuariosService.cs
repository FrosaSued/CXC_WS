using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using CXC_WS.Controllers;
using CXC_WS.Data.Models;
using CXC_WS.Interfaces;

namespace CXC_WS.Services
{
    public class UsuariosService : IUsuario
    {
        private readonly cxc_newContext _context;
        public UsuariosService(cxc_newContext context) => _context = context;

        public async Task<Usuario> GetUser(Usuario usuario)
        {

            var query = (from a in _context.Usuarios
                         where a.Usuario1 == usuario.Usuario1
                         select a).FirstOrDefault();

            if (query != null)
            {
                var validarpassword = ValidarPassword(usuario.Password, query.Password, query.Salt);

                if (validarpassword)
                {
                    /*Log log = new Log();
                    log.Usuario = "401399";
                    log.Descripcion = "Usuario logueado";
                    log.Fecha= DateTime.Now;
                    _context.Logs.Add(log);
                    _context.SaveChanges();*/
                    return query;

                }
                /*else
                {
                    return null;
                }*/
            }

            return null;

        }
        public async Task<Usuario> Register(Usuario usuario)
        {
            Usuario usertbl = new Usuario();
            var passwordog = usuario.Password;

            var salt = CrearSalt();
            var passwordhashed = GenerarHash(passwordog, salt);
            var user = usuario;
            var password = usuario.Password;
            var nombre = usuario.Nombre;
            var rol = usuario.Rol;

            usertbl.Usuario1 = usuario.Usuario1;
            usertbl.Password = passwordhashed;
          //  usertbl.Password2 = passwordhashed;
            usertbl.Salt = salt;
            usertbl.Nombre = nombre;
            usertbl.Rol = rol;
            usertbl.Estado = 3;
            _context.Usuarios.AddAsync(usertbl);
            _context.SaveChanges();

            return usertbl;
        }


        public static string CrearSalt()
        {
            var rng = RandomNumberGenerator.Create();
            byte[] buff = new byte[128 / 8];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }


        public static string GenerarHash(string input, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input + salt);
            SHA256 sHA256ManagedString = SHA256.Create();
            byte[] hash = sHA256ManagedString.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public bool ValidarPassword(string password, string stringHash, string salt)
        {
            string pass = password.ToString();
            string newHashedPin = GenerarHash(pass, salt);
            return newHashedPin.Equals(stringHash);
        }

        public async Task<List<Usuario>> GetAllUsuarios()
        {
            var ust = (from a in _context.Usuarios select new Usuario { Usuario1 = a.Usuario1, Nombre = a.Nombre, Rol = a.Rol, Estado = a.Estado }).ToListAsync();
            return await ust;
        }
        

    }
}