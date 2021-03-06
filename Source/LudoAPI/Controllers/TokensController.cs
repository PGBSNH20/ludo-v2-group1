using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Ludo.API.Data;

namespace Ludo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly ITokenRepo _tokenRepo;

        public TokensController(ITokenRepo tokenRepo)
        {
            _tokenRepo = tokenRepo;
        }
        [HttpPut("{tokenId}")]
        public async Task<IActionResult> RollDice(int tokenId, int diceNumber)
        {
            if (diceNumber > 6 || diceNumber < 1) return BadRequest();
            var result = await _tokenRepo.MoveToken(tokenId, diceNumber);
            if (result == "BadRequest") return BadRequest();
            return Ok(JsonSerializer.Serialize($"{result}"));
        }
    }
}
