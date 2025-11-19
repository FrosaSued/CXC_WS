using System;
using System.Collections.Generic;

namespace CXC_WS.Data.Models
{
    public partial class TipoPago
    {
        public TipoPago()
        {
            Pagos = new HashSet<Pago>();
        }

        public int Id { get; set; }
        public string Descripcion { get; set; } = null!;

        public virtual ICollection<Pago> Pagos { get; set; }
    }
}
