using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Ludo.Web;
using Ludo.Web.Pages;
using LudoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Xunit;

namespace Ludo.Test
{
    public class PagesTests
    {
        [Fact]
        public async Task OnGetAsync_Populates_PageModel_With_BoardAsync()
        {
            var fakeBoard = new Board { BoardName = "TestBoard" };
            var ludoData = A.Fake<ILudoDataAccess>();
            var pageModel = new GameModel(ludoData);

            A.CallTo(() => ludoData.GetGameAsync("TestGame")).Returns(fakeBoard);

            var result = await pageModel.OnGetAsync("TestGame");

            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task OnPostAsync_ReturnsA_RedirectTo_PageResult()
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
        public async Task OnPostAsync_BadRequest_Returns_Page()
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
    }
}
