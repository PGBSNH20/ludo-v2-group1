using FakeItEasy;
using Ludo.API.Controllers;
using Ludo.API.Data;
using Ludo.API.Logic;
using LudoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ludo.API.Models;
using Xunit;

namespace Ludo.Test
{
    public class TokensControllerTests
    {
        [Fact]
        public async Task When_Move_Token_Expect_Ok()
        {
            var tokenRepo = A.Fake<ITokenRepo>();
            var controller = new TokensController(tokenRepo);
            var diceNumber = 6;
            var tokenID = 1;
            string resultOfMovement = "You made a move!";
            A.CallTo(() => tokenRepo.MoveToken(tokenID, diceNumber)).Returns(resultOfMovement);

            var actionResult = await controller.RollDice(tokenID, diceNumber);
         
            Assert.IsType<OkObjectResult>(actionResult);
        }

        [Fact]
        public async Task When_Move_Token_With_Bad_Dice_Number_Expect_BadRequest()
        {
            var tokenRepo = A.Fake<ITokenRepo>();
            var controller = new TokensController(tokenRepo);
            var diceNumber = 7;
            var tokenID = 1;
            string resultOfMovement = "You made a move!";
            A.CallTo(() => tokenRepo.MoveToken(tokenID, diceNumber)).Returns(resultOfMovement);

            var actionResult = await controller.RollDice(tokenID, diceNumber);
            
            Assert.IsType<BadRequestResult>(actionResult);
        }
        [Fact]
        public async Task When_Move_Non_Existing_Token_With_Expect_BadRequest()
        {
            var tokenRepo = A.Fake<ITokenRepo>();
            var controller = new TokensController(tokenRepo);
            var diceNumber = 6;
            var tokenID = 0;
            string resultOfMovement = "BadRequest";
            A.CallTo(() => tokenRepo.MoveToken(tokenID, diceNumber)).Returns(resultOfMovement);

            var actionResult = await controller.RollDice(tokenID, diceNumber);

            Assert.IsType<BadRequestResult>(actionResult);
        }
    }
}
