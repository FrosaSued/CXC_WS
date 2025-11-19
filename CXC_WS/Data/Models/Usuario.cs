using System;
using System.Collections.Generic;

namespace CXC_WS.Data.Models
{
    public partial class Usuario
    {
        public int? Id { get; set; }
        public string Usuario1 { get; set; } = null!;
        public string? Nombre { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int? Estado { get; set; }
        public int? Rol { get; set; }
        public string? Salt { get; set; }
                public string? Correo { get; set; }

    }
    
    public class Jwt
    {
        public string key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }

    }
}
