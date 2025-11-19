using System;
using System.Collections.Generic;

namespace CXC_WS.Data.Models
{
    public partial class Pago
    {
        public int Id { get; set; }
      //  public int? IdDocumento { get; set; }
        public int? DocumentoId { get; set; }
        public int? TipoPagoId { get; set; }
        public decimal? Monto { get; set; }
        public DateTime? FechaPago { get; set; }
        public string? Comentario { get; set; }
        public DateTime? FechaCreacion { get; set; }

        //public virtual Documento? IdDocumentoNavigation { get; set; }
      //  public virtual TipoPago? IdtipoPagoNavigation { get; set; }
    }
}
