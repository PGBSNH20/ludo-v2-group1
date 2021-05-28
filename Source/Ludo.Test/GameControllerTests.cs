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
    public class GameControllerTests
    {
        [Fact]
        public async Task When_Getting_Game_By_Name_Expect_OKAsync()
        {
            var gameRepo = A.Fake<IGameRepo>();
            var controller = new GameController(gameRepo);
            Board game = GameFactory.CreateBoard("testGame");
            Player newPlayer = GameFactory.NewPlayer("newPlayer", game, TokenColor.Red);
            game.Players.Add(newPlayer);

            A.CallTo(() => gameRepo.GetGameByName("testGame")).Returns(game);

            var actionResult = await controller.GetGameByName("testGame");

            Assert.IsType<OkObjectResult>(actionResult.Result);
        }
        [Fact]
        public async Task When_Getting_Game_By_Bad_Name_Expect_NotFound()
        {
            var gameRepo = A.Fake<IGameRepo>();
            var controller = new GameController(gameRepo);
            Board returnValue = null;

            A.CallTo(() => gameRepo.GetGameByName("qwerty")).Returns(returnValue);

            var actionResult = await controller.GetGameByName("qwerty");

            Assert.IsType<NotFoundResult>(actionResult.Result);
        }
    }
}
