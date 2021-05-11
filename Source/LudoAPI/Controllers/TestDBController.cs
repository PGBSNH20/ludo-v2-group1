using LudoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LudoAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestDBController : ControllerBase // test connection to DB
    {
        private readonly LudoContext _context;

        public TestDBController(LudoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<Board> GetTest()
        {
            var list = _context.Board.ToList();
            return list;
        }
    }
}
