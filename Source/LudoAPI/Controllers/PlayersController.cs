using Ludo.API.Data;
using LudoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

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
        public async Task<IActionResult> PostPlayer(string gameName, PlayerTokenColor playerTokenColor)
        {
            var result = await _playerRepo.AddPlayer(playerTokenColor.PlayerName, gameName, playerTokenColor.TokenColor);
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
            await _playerRepo.AddPlayerTurnName(gameName, player);
            return Ok();
        }
        [HttpGet("turn/{gameName}")]
        public async Task<IActionResult> GetPlayerTurn(string gameName, string player)
        {
            string name = await _playerRepo.GetPlayerTurn(gameName);
            return Ok(name);
        }

        [HttpPut("dice/{gameName}")]
        public async Task<IActionResult> RollDice(string gameName, [FromBody]string player, [FromQuery]int diceNumber)
        {
            var result = await _playerRepo.MovePlayer(gameName, player, diceNumber);
            if (result.IsCompleted)
                return Ok(JsonSerializer.Serialize($"{player} moved {diceNumber} steps"));
            return BadRequest();

        }
    }
}
