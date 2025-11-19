using System;
using System.Collections.Generic;

namespace CXC_WS.Data.Models
{
    public partial class Logs
    {
        public int Id { get; set; }
        public string Usuario { get; set; } = null!;
        public string? Descripcion { get; set; }
        public DateTime? Fecha { get; set; }
    }
}