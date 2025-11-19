using System;
using System.Collections.Generic;

namespace CXC_WS.Data.Models
{
    public partial class DocumentosDetalle
    {
        public int Id { get; set; }
        public int? IdDocumento { get; set; }
        public decimal? Monto { get; set; }
        public string? ComentarioDetalle { get; set; }
        public int? Estado { get; set; }
        public decimal? MontoPendiente { get; set; }
        public decimal? MontoPagar { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }
}
