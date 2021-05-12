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
    public class PlayerController : Controller
    {
        private readonly IPlayerRepo _playerRepo;

        public PlayerController(IPlayerRepo playerRepo)
        {
            _playerRepo = playerRepo;
        }
        // POST: api/Player?playerName=Bob&gameName=game1&color=red
        // Adds a new player to an existing game
        [HttpPost]
        public async Task<IActionResult> PostPlayer(string playerName, string gameName, string color)
        {
            var result = await _playerRepo.AddPlayer(playerName, gameName, color);
            if (result.Exception != null)
                return BadRequest(result.Exception.Message);
            return Ok();
        }
    }
}
