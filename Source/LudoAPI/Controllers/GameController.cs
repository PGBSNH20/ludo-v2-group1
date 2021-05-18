using Ludo.API.Data;
using LudoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ludo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameRepo _gameRepo;

        public GameController(IGameRepo gameRepo)
        {
            _gameRepo = gameRepo;
        }
        // GET: api/Game/ludoBoard
        [HttpGet("{boardName}")]
        public async Task<ActionResult<Board>> GetGameByName(string boardName)
        {
            var board = await _gameRepo.GetGameByName(boardName);

            if (board == null)
                return NotFound();
            return Ok(board);
        }
    }
}
