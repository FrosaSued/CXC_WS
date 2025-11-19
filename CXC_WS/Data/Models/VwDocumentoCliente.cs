using System;
using System.Collections.Generic;

namespace CXC_WS.Data.Models
{
    public partial class VwDocumentoCliente
    {
        public int Id { get; set; }
        public int? TipoDoc { get; set; }
        public DateTime? FechaDoc { get; set; }
        public int? IdCliente { get; set; }
        public bool? Saldado { get; set; }
        public string? Comentario { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public int? Estado { get; set; }
        public string? ClienteNombre { get; set; }
        public string? Condiciones { get; set; }

        public decimal? Monto_Pagar { get; set; }
        public decimal? Monto_Pendiente { get; set; }
        public List<Concepto> Conceptos { get; set; }

    }
}
