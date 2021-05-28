using Ludo.API.Data;
using LudoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Ludo.API.Models;

namespace Ludo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerRepo _playerRepo;

        public PlayersController(IPlayerRepo playerRepo)
        {
            _playerRepo = playerRepo;
        }
        // POST: api/Players/{gameName}
        // Adds a new player to an existing game
        [HttpPost("{gameName}")]
        public async Task<IActionResult> PostPlayer(string gameName, PlayerTokenColor player)
        {
            var result = await _playerRepo.AddPlayer(player.PlayerName, gameName, player.TokenColor);
            if (result.Exception != null)
                return BadRequest(result.Exception.Message);
            return Ok();
        }

        [HttpGet("{gameName}")]
        public async Task<IActionResult> GetPlayers(string gameName)
        {
            var result = await _playerRepo.GetPlayers(gameName);
            return Ok(result);
        }

        [HttpPut("turn/{gameName}")]
        public async Task<IActionResult> AddPlayerTurn(string gameName, [FromBody]string player)
        {
            var result = await _playerRepo.AddPlayerTurnName(gameName, player);
            if (result == null) return BadRequest();
            return Ok();
        }
        [HttpGet("turn/{gameName}")]
        public async Task<IActionResult> GetPlayerTurn(string gameName)
        {
            string name = await _playerRepo.GetPlayerTurn(gameName);
            if (string.IsNullOrEmpty(name)) return BadRequest();
            return Ok(name);
        }
        [HttpPut("dice/{tokenId}")]
        public async Task<IActionResult> RollDice(int tokenId, int diceNumber)
        {
            var result = await _playerRepo.MovePlayer(tokenId, diceNumber);
            return Ok(JsonSerializer.Serialize($"{result}"));
        }
    }
}
