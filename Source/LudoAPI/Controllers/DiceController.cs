using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ludo.API.Logic;
using LudoAPI.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Ludo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiceController : ControllerBase
    {
        //api/dice
        [HttpGet]
        public IActionResult RollDice()
        {
            return Ok(Dice.RollDice());
        }
    }
}
