using CXC_WS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CXC_WS.Interfaces
{
    public interface IUsuario
    {
        Task<List<Usuario>> GetAllUsuarios();

        Task<Usuario> GetUser(Usuario usuario);
        Task<Usuario> Register(Usuario usuario);
        // Task<Usuario> PutUsuario(int id, Usuario usuario);

        //  public bool ValidarContrasena(string password, string stringHash, string salt);


    }
}