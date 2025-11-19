using System;
using System.Collections.Generic;

namespace CXC_WS.Data.Models
{
    public partial class Cliente
    {
      /*  public Cliente()
        {
            Documentos = new HashSet<Documento>();
        }*/

        public int? Id { get; set; }
        public string? Nombre { get; set; }
        public string? Direccion { get; set; }
        public string? Ciudad { get; set; }
        public string? Pais { get; set; }
        public string? Telefono { get; set; }
        public string? Nif { get; set; }
        public string? Condiciones { get; set; }
        public string? CuentaBanco { get; set; }
        public int? Estado { get; set; }
        public string? EstadoPais { get; set; }
        public string? Moneda { get; set; }

     //   public virtual ICollection<Documento> Documentos { get; set; }
    }
}
