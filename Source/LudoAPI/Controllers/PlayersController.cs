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
    public class PlayersController : Controller
    {
        private readonly IPlayerRepo _playerRepo;

        public PlayersController(IPlayerRepo playerRepo)
        {
            _playerRepo = playerRepo;
        }
        // POST: api/Players/{gameName}
        // Adds a new player to an existing game
        [HttpPost("{gameName}")]
        public async Task<IActionResult> PostPlayer(string gameName, PlayerTokenColor playerTokenColor)
        {
            var result = await _playerRepo.AddPlayer(playerTokenColor.PlayerName, gameName, playerTokenColor.TokenColor);
            if (result.Exception != null)
                return BadRequest(result.Exception.Message);
            return Ok();
        }
    }
}
