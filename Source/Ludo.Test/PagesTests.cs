using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Ludo.API.Models;
using Ludo.Web;
using Ludo.Web.Pages;
using LudoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestSharp;
using Xunit;

namespace Ludo.Test
{
    public class PagesTests
    {
        [Fact]
        public async Task Game_OnGetAsync_Populates_PageModel_With_BoardAsync()
        {
            var fakeBoard = new Board { BoardName = "TestBoard" };
            var ludoData = A.Fake<ILudoDataAccess>();
            var pageModel = new GameModel(ludoData);

            A.CallTo(() => ludoData.GetGameAsync("TestGame")).Returns(fakeBoard);

            var result = await pageModel.OnGetAsync("TestGame");

            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task NewGame_OnPostAsync_ReturnsA_RedirectTo_PageResult()
        {
            var ludoData = A.Fake<ILudoDataAccess>();
            var pageModel = new NewGameModel(ludoData);
            Board board = new Board { BoardName = "TestBoard", Id = 1 };
            pageModel.Board = board;

            A.CallTo(() => ludoData.GetBoardAsync("TestBoard")).Returns(board);

            A.CallTo(() => ludoData.AddBoard("Testboard"))
                .Returns(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

            var result = await pageModel.OnPostAsync();

            Assert.IsType<RedirectToPageResult>(result);
        }
        [Fact]
        public async Task NewGame_OnPostAsync_BadRequest_Returns_Page()
        {
            var ludoData = A.Fake<ILudoDataAccess>();
            var pageModel = new NewGameModel(ludoData);
            Board board = new Board { BoardName = "TestBoard", Id = 1 };
            pageModel.Board = board;

            A.CallTo(() => ludoData.GetBoardAsync("TestBoard")).Returns(board);

            A.CallTo(() => ludoData.AddBoard("Testboard"))
                .Returns(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest });

            var result = await pageModel.OnPostAsync();

            Assert.IsType<RedirectToPageResult>(result);
        }
        [Fact]
        public async Task Player_OnPostAddPlayerAsync_Returns_PageResult()
        {
            var ludoData = A.Fake<ILudoDataAccess>();
            var pageModel = new PlayerModel(ludoData);
            Board board = new Board { BoardName = "TestBoard", Id = 1 };
            PlayerTokenColor player = new PlayerTokenColor { PlayerName = "Bob", TokenColor = "Red" };
            pageModel.Board = board;
            pageModel.NameColor = player;
            A.CallTo(() => ludoData.PostPlayer("TestBoard", player)).Returns(new RestResponse { StatusCode = HttpStatusCode.OK });
            A.CallTo(() => ludoData.GetGameAsync("Testboard")).Returns(board);

            var result = await pageModel.OnPostAddPlayerAsync("TestBoard");

            Assert.IsType<PageResult>(result);
        }
        [Fact]
        public async Task Player_OnPostAddPlayerAsync_Bad_Game_Name_Returns_RedirectPageResult()
        {
            var ludoData = A.Fake<ILudoDataAccess>();
            var pageModel = new PlayerModel(ludoData);
            Board board = new Board { BoardName = "BadGameName", Id = 1 };
            PlayerTokenColor player = new PlayerTokenColor { PlayerName = "Bob", TokenColor = "Red" };
            pageModel.Board = board;
            pageModel.NameColor = player;
            Board BadBoard = null;  
            A.CallTo(() => ludoData.PostPlayer("BadGameName", player)).Returns(new RestResponse { StatusCode = HttpStatusCode.BadRequest });
            A.CallTo(() => ludoData.GetGameAsync("BadGameName")).Returns(BadBoard);

            var result = await pageModel.OnPostAddPlayerAsync("BadGameName");

            Assert.IsType<RedirectToPageResult>(result);
        }
        [Fact]
        public async Task Player_OnGetAsync_Returns_PageResult()
        {
            var ludoData = A.Fake<ILudoDataAccess>();
            var pageModel = new PlayerModel(ludoData);
            Board board = new Board { BoardName = "TestBoard", Id = 1 };
            pageModel.Board = board;
            A.CallTo(() => ludoData.GetGameAsync("TestBoard")).Returns(board);

            var result = await pageModel.OnPostAddPlayerAsync("TestBoard");

            Assert.IsType<PageResult>(result);
        }
        [Fact]
        public async Task Player_OnGetAsync_Bad_Game_Name_Returns_RedirectPageResult()
        {
            var ludoData = A.Fake<ILudoDataAccess>();
            var pageModel = new PlayerModel(ludoData);
            Board board = new Board { BoardName = "BadGameName", Id = 1 };
            pageModel.Board = board;
            Board BadBoard = null;
            A.CallTo(() => ludoData.GetGameAsync("BadGameName")).Returns(BadBoard);

            var result = await pageModel.OnPostAddPlayerAsync("BadGameName");

            Assert.IsType<RedirectToPageResult>(result);
        }
    }
}
