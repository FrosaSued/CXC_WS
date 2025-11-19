using System;
using System.Collections.Generic;

namespace CXC_WS.Data.Models
{
    public partial class Concepto
    {
        public int Id { get; set; }
        public int? IdDocDetalle { get; set; }
        public int? DocumentoID { get; set; }
        public DateTime? FechaDoc { get; set; }
        public string? Concepto1 { get; set; }
        public decimal? Monto { get; set; }
        public int? Estado { get; set; }
        public bool? Manual { get; set; }
        
       // public int? IdDocDetalle { get; set; } // Foreign key
       // public Documento? Documento { get; set; }
       // public virtual Documento? IdDocumentoNavigation { get; set; }

    }
}
