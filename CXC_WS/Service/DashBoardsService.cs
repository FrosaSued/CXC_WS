using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CXC_WS.Data.DTOs;
using CXC_WS.Data.Models;
using CXC_WS.Interfaces;

namespace CXC_WS.Services
{
    public class DashBoardsService : IDashBoards
    {
        private readonly cxc_newContext _context;

        public DashBoardsService(cxc_newContext context)
        {
            _context = context;
        }
        
        public async Task<List<Documento>> GetDetalleDocumetosByEstado2(int estado)
        {
            return await _context.Documentos.Where(x => x.Estado == estado).ToListAsync();
        }
        
    }
}