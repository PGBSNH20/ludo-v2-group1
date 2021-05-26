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
    public class PlayerControllerTests
    {
        [Fact]
        public async Task When_Posting_Player_Expect_OKAsync()
        {
            var playerRepo = A.Fake<IPlayerRepo>();
            var controller = new PlayersController(playerRepo);
            
            var playerTokenColor = new PlayerTokenColor { PlayerName = "player1", TokenColor = "red" };  
            string gameName = "game1";

            A.CallTo(() => playerRepo.AddPlayer(playerTokenColor.PlayerName, gameName, playerTokenColor.TokenColor))
                .Returns(Task.CompletedTask);

            var actionResult = await controller.PostPlayer(gameName, playerTokenColor);

            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        public async Task When_Posting_Bad_Player_Expect_BadRequestAsync()
        {
            var playerRepo = A.Fake<IPlayerRepo>();
            var controller = new PlayersController(playerRepo);
       
            var playerTokenColor = new PlayerTokenColor { PlayerName = "player1", TokenColor = "red" };
            string gameName = "asdfa";
       
            A.CallTo(() => playerRepo.AddPlayer(playerTokenColor.PlayerName, gameName, playerTokenColor.TokenColor))
                .Returns(Task.FromException(new ArgumentException("Bad request")));

            var actionResult = await controller.PostPlayer(gameName, playerTokenColor);

            Assert.IsType<BadRequestObjectResult>(actionResult);
        }
    }
}
