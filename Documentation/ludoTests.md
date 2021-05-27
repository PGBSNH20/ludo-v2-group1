## Ludo Test Documentation

A overview of tests used in our Ludo project, all our tests are based on repository interfaces for good isolation and testing.

Using **xUnit** and **FakeItEasy** we can test the behavior of components in our project, below are the different areas we have tests for along with an example.

### Controller Tests

Several tests exist for testing the behavior and return message (**IActionResult**) of our controllers when passing data to them, under is an example of how the tests look.

#### Board Controller Test Example

```c#
 [Fact]
        public async Task When_Getting_Non_Existant_Board_By_Name_Expect_NotFound()
        {
            var boardRepo = A.Fake<IBoardRepo>();
            var controller = new BoardController(boardRepo);
            Board returnValue = null;
// Simulate sending GET request for a board that does not exist in the database
            A.CallTo(() => boardRepo.GetBoardByName("a")).Returns(returnValue); 
            var actionResult = await controller.GetBoardByName("a");

            Assert.IsType<NotFoundResult>(actionResult.Result);
        }
```

#### Player Controller Test Example

```c#
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
```

### Razor Pages Test

Using the same libraries we can also test our razor pages behavior, for example `PageResult`.

####  Razor Page Test Example

```c#
[Fact]
        public async Task OnPostAsync_ReturnsA_RedirectTo_PageResult()
        {
            // Method simulates the backend OnPost method of the razor page "NewGame"
            var ludoData = A.Fake<ILudoDataAccess>();
            var pageModel = new NewGameModel(ludoData); // Create the pagemodel
            Board board = new Board { BoardName = "TestBoard", Id = 1 };
            pageModel.Board = board;

            A.CallTo(() => ludoData.GetBoardAsync("TestBoard")).Returns(board);

            A.CallTo(() => ludoData.AddBoard("Testboard"))
                .Returns(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

            var result = await pageModel.OnPostAsync();

            Assert.IsType<RedirectToPageResult>(result);
        }
```

