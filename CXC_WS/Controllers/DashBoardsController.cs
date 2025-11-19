using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CXC_WS.Data.Models;
using CXC_WS.Interfaces;
using Microsoft.AspNetCore.Authorization;
using CXC_WS.Data.DTOs;
using Microsoft.EntityFrameworkCore.Query;


namespace CXC_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class DashBoardsController : ControllerBase
    {
        private readonly IDashBoards _dashBoards;
        private readonly cxc_newContext _context;

        public DashBoardsController(cxc_newContext context, IDashBoards dashBoards)
        {
            _context = context;
            _dashBoards = dashBoards;
        }
        // GET: api/Estados
        [HttpGet]
        [Route("/api/DashBoards/GetCountDocumentos/")]
        public async Task<ActionResult<List<DashboardDTO>>> GetCountDocumentos()
        {
            var CounterIventario =  from item in _context.Documentos
                group item by item.Estado into g
                select new DashboardDTO()
                {
                    Cantidad = g.Select(x => x.Id).Distinct().Count(),
                    Estado = g.Key
                };
            
            return await CounterIventario.ToListAsync();
        }
        [HttpGet]
        [Route("/api/DashBoards/GetCountDocumentosCompletados/")]
        public async Task<ActionResult<List<DashboardDTO>>> GetCountDocumentosCompletados()
        {
            
            var CounterIventario =  from item in _context.Documentos.Where(x=>x.FechaSaldo >= DateTime.Today.AddDays(-30) && x.Estado == 6)
                group item by item.Estado into g
                select new DashboardDTO()
                {
                    Cantidad = g.Select(x => x.Id).Distinct().Count(),
                    Estado = g.Key
                };
            
            return await CounterIventario.ToListAsync();
        }
        
        [HttpGet]
        [Route("/api/DashBoards/GetCountDocumentosAnulados/")]
        public async Task<ActionResult<List<DashboardDTO>>> GetCountDocumentosAnulados()
        {
            
            var CounterIventario =  from item in _context.Documentos.Where(x=>x.FechaSaldo >= DateTime.Today.AddDays(-30) && x.Estado == 4)
                group item by item.Estado into g
                select new DashboardDTO()
                {
                    Cantidad = g.Select(x => x.Id).Distinct().Count(),
                    Estado = g.Key
                };
            
            return await CounterIventario.ToListAsync();
        }
       
    }
}