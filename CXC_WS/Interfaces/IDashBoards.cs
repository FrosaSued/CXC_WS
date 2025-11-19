using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CXC_WS.Data.DTOs;
using CXC_WS.Data.Models;


namespace CXC_WS.Interfaces
{
    public interface IDashBoards
    {
        public Task<List<Documento>> GetDetalleDocumetosByEstado2(int estado);
        // public Task<List<DashboardDTO>> GetAllDetalleDocumentos();

    }
}