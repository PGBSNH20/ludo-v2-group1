using System;
using System.Collections.Generic;
using FakeItEasy;
using Ludo.API.Controllers;
using Ludo.API.Data;
using Ludo.API.Logic;
using LudoAPI.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Ludo.Test
{
    public class BoardControllerTests
    {

        [Fact]
        public async Task When_Getting_All_Boards_Expect_OKAsync()
        {
            var boardRepo = A.Fake<IBoardRepo>();
            var board = GameFactory.CreateBoard("TestBoard");
            var fakeList = new List<Board>();
            var controller = new BoardController(boardRepo);
            
            fakeList.Add(board);
            A.CallTo(() => boardRepo.GetAllBoards()).Returns(fakeList);
            
            var actionResult = await controller.GetAllBoards();

            Assert.IsType<OkObjectResult>(actionResult.Result);
        }
        [Fact]
        public async Task When_Getting_All_Boards_But_No_Boards_Exist_Expect_No_Content()
        {
            var boardRepo = A.Fake<IBoardRepo>();
            var board = GameFactory.CreateBoard("TestBoard");
            var fakeList = new List<Board>();
            var controller = new BoardController(boardRepo);

            A.CallTo(() => boardRepo.GetAllBoards()).Returns(fakeList);

            var actionResult = await controller.GetAllBoards();

            Assert.IsType<NoContentResult>(actionResult.Result);
        }
        [Fact]
        public async Task When_Getting_Non_Existant_Board_By_Name_Expect_NotFound()
        {
            var boardRepo = A.Fake<IBoardRepo>();
            var controller = new BoardController(boardRepo);
            Board returnValue = null;

            A.CallTo(() => boardRepo.GetBoardByName("a")).Returns(returnValue);
            var actionResult = await controller.GetBoardByName("a");

            Assert.IsType<NotFoundResult>(actionResult.Result);
        }
        [Fact]
        public async Task When_Getting_Board_By_Name_Expect_Ok()
        {
            var boardRepo = A.Fake<IBoardRepo>();
            var controller = new BoardController(boardRepo);
            var board = GameFactory.CreateBoard("TestBoard");

            A.CallTo(() => boardRepo.GetBoardByName("a")).Returns(board);
            var actionResult = await controller.GetBoardByName("a");

            Assert.IsType<OkObjectResult>(actionResult.Result);
        }
        [Fact]
        public async Task When_Updating_Board_With_Wrong_Id_Expect_Bad_Request()
        {
            var boardRepo = A.Fake<IBoardRepo>();
            var controller = new BoardController(boardRepo);
            var board = GameFactory.CreateBoard("TestBoard");

            A.CallTo(() => boardRepo.UpdateBoard(123, board)).Returns(Task.FromException(new ArgumentException("Id does not match")));

            var actionResult = await controller.UpdateBoard(123, board);

            Assert.IsType<BadRequestResult>(actionResult);
        }
        [Fact]
        public async Task When_Updating_Board_Expect_Ok()
        {
            var boardRepo = A.Fake<IBoardRepo>();
            var controller = new BoardController(boardRepo);
            var board = GameFactory.CreateBoard("TestBoard");

            A.CallTo(() => boardRepo.UpdateBoard(1, board))
                .Returns(Task.CompletedTask);

            var actionResult = await controller.UpdateBoard(1, board);

            Assert.IsType<OkResult>(actionResult);
        }
        [Fact]
        public async Task When_Adding_Bad_Board_Expect_Bad_Request()
        {
            var boardRepo = A.Fake<IBoardRepo>();
            var controller = new BoardController(boardRepo);
            var board = GameFactory.CreateBoard("asdfg");

            A.CallTo(() => boardRepo.AddBoard(board)).Returns(Task.FromException(new ArgumentException("Bad request")));

            var actionResult = await controller.AddBoard(board);

            Assert.IsType<BadRequestResult>(actionResult.Result);
        }
        [Fact]
        public async Task When_Deleting_Bad_Board_Expect_Not_Found()
        {
            var boardRepo = A.Fake<IBoardRepo>();
            var controller = new BoardController(boardRepo);

            A.CallTo(() => boardRepo.DeleteBoard("asdgd")).Returns(Task.FromException(new ArgumentException("Not found")));

            var actionResult = await controller.DeleteBoard("asdgd");

            Assert.IsType<NotFoundResult>(actionResult);
        }
        [Fact]
        public async Task When_Deleting_Board_Expect_Ok()
        {
            var boardRepo = A.Fake<IBoardRepo>();
            var controller = new BoardController(boardRepo);

            A.CallTo(() => boardRepo.DeleteBoard("asdgd")).Returns(Task.CompletedTask);

            var actionResult = await controller.DeleteBoard("asdgd");

            Assert.IsType<OkResult>(actionResult);
        }


    }
}
