using FakeItEasy;
using Ludo.API.Controllers;
using Ludo.API.Data;
using Ludo.API.Logic;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ludo.Test
{
    public class PlayerControllerTests
    {
        //[Fact]
        //public async Task When_Posting_Player_Expect_OKAsync()
        //{
        //    var playerRepo = A.Fake<IPlayerRepo>();
        //    var controller = new PlayersController(playerRepo);

        //    string playerName = "player1";
        //    string gameName = "game1";
        //    string color = "red";
        //    A.CallTo(() => playerRepo.AddPlayer(playerName, gameName, color))
        //        .Returns(Task.CompletedTask);

        //    var actionResult = await controller.PostPlayer(playerName, gameName, color);

        //    Assert.IsType<OkResult>(actionResult);
        //}

        //[Fact]
        //public async Task When_Posting_Bad_Player_Expect_BadRequestAsync()
        //{
        //    var playerRepo = A.Fake<IPlayerRepo>();
        //    var controller = new PlayersController(playerRepo);

        //    string playerName = "player1";
        //    string gameName = "asdfa";
        //    string color = "red";
        //    A.CallTo(() => playerRepo.AddPlayer(playerName, gameName, color))
        //        .Returns(Task.FromException(new ArgumentException("Bad request")));

        //    var actionResult = await controller.PostPlayer(playerName, gameName, color);

        //    Assert.IsType<BadRequestObjectResult>(actionResult);
        //}
    }
}
