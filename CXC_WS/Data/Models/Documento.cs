using System;
using System.Collections.Generic;

namespace CXC_WS.Data.Models
{
    public partial class Documento
    {
       /* public Documento()
        {
            Pagos = new HashSet<Pago>();
          //  Conceptos = new List<Concepto>();

        }*/

        public int Id { get; set; }
        public int? TipoDoc { get; set; }
        public DateTime? FechaDoc { get; set; }
        public int? IdCliente { get; set; }
        public string? NombreCliente { get; set; }
        public bool? Saldado { get; set; }
        public string? Condiciones { get; set; }
        public string? Moneda { get; set; }

        public string? Comentario { get; set; }
        public decimal? Monto_Pagar { get; set; }
        public decimal? Monto_Pendiente { get; set; }

        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaSaldo { get; set; }
        public int? Estado { get; set; }
     //   public virtual Cliente? IdClienteNavigation { get; set; }
       // public virtual ICollection<Pago> Pagos { get; set; }
       // public virtual ICollection<Concepto> Conceptos { get; set; }
       public List<Concepto>? Conceptos { get; set; }
       public List<Pago>? Pagos { get; set; }


    }
}
