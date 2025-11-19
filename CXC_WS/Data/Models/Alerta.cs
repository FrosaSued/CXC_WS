using System;
using System.Collections.Generic;
using CXC_WS.Data.Models;

namespace CXC_WS.Data.Models
{
    public partial class Alerta
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public string IdTipoDocumento { get; set; } = null!;
        public short DiasAlerta { get; set; }
        public string UsuarioCreacion { get; set; } = null!;
        public DateTime FhCreacion { get; set; }
        public bool? Activa { get; set; }
    }

}
