## Ludo API
### Controllers Layer

Endpoints for controllers are described here --> [Controller Endpoints](ludoApiEndpoints.md)

The controllers pass through a repository layer, which returns what the controller asks for. Repo methods are documented at the bottom.

#### Board Controller

The Board controller is responsible for handling requests regarding the actual board class. The following methods have been implemented.

* ``GetAllBoards`` - Returns a list of all available boards from the database
* `GetBoardByName(string boardName)` - Returns a specific board from the database
* ``UpdateBoard(int id, Board board)`` - Updates a board
* ``AddBoard([FromBody] string boardName)`` - Adds a new board to the database
* ``DeleteBoard(string boardName)``- Deletes the board from the database 

#### Dice Controller

The dice controller returns a random number between **1** and **6**. 

* ``RollDice`` - Returns a random number

#### Game Controller

The game controller is a complete game that returns a board, the players connected to it and their tokens.

* ``GetGameByName`` - Returns a board that includes ``Player`` and  ``Token`` from the database

#### Players Controller

The player controller handles the actions regarding the players, such as adding a new player to a board.

* ``PostPlayer`` - Calls ``AddPlayer`` from ``PlayerRepo`` that adds the ``Player`` to the specified board.
* ``GetPlayers(string gameName)`` - Calls ``GetPlayers`` from ``PlayerRepo`` that returns a list of all players related to the specified board.
* ``AddPlayerTurn(string gameName, [FromBody] string player)`` - Calls ``AddPlayerTurn`` from ``PlayerRepo`` that updates who has the next turn in the database.
* ``GetPlayerTurn(string gameName, string player)`` - Calls ``GetPlayerTurn`` from ``PlayerRepo`` that returns the name of the player who has the next turn from the database

#### Tokens Controller
* ``RollDice(int tokenId, int diceNumber)`` - Calls ``MovePlayer`` from ``TokenRepo`` that initiates the moving and returns a string result.

### Models Layer
#### Board Class
The main class that stores all the information about the game.

- ``BoardName`` - string property for name of board.
- ``List`` of ``Squares`` stores all the squares. See below.
- ``List`` of ``Players``store all the players. See below.
- ``Id`` property of type int. It is used as PK in the database.
- ``PlayerTurnName`` of type string. Used to determine who has the next turn.

#### Square Class
Game board consists of 76 squares. Square number (Id) can be determined in the figure:   
[<img src="https://github.com/PGBSNH20/ludo-v2-group1/blob/main/Documentation/LudoApiDocs/SquareNumbering.jpg">](SquareNumbering)

- Id property of type int. It determines the square number. It can be 0-51, 101-106, 201-206, 301-306, 401-406.
- Property Occupants is List of Tokens. It stores tokens that are on this square. Token- see below.      

#### Player Class
The object that represents one of max 4 players.

- `Name` property of type string.
- An array of 4 tokens where one will be active at start. Token- see below.
- `BoardId` - For database relation
- `Id` property of type int. It is used as PK in the database.

#### Token Class
Each player has 4 tokens, if the dice hits a 6, the user can either put a new token in play, if all are not already active. Or move an existing one 6 times instead.

- `IsActive` property of type bool. False - token is on the base, true - token is in play. If a token has reached finish it removes from the game.   
- `Color` property of type `TokenColor` (enum).
- `Steps` property of type int. It keeps track of where the token is. A token needs to take 56 steps to reach the finish.
- `Route` array of type int, to describe which squares the token will be following. Route depends on tokens color.
- `Id` property of type int. It is used as PK in the database.
- `PlayerID` property of type int. It determints the player who owns the token, is used as FK in the database. 
- `Move`(Board board, Player player, int dice) - Method for moving the token x amount of steps depending on the dice result.<br><br>
Using `Steps` and `Route` you can determine the `squareID` on which the token is.
[<img src="https://github.com/PGBSNH20/ludo-v2-group1/blob/main/Documentation/LudoApiDocs/Routes.jpg">](Routes)

### Logic Layer
#### Dice Class
A dice consisting of numbers between 1 and 6.
- `RollDice`- Method that generates a random number between 1 and 6
#### GameFactory Class
Class is used to create the desired objects

- `CreateBoard` - Creates a board, call CreateSquares() and gets a list of squares, adds this list of squares to the board.
- `CreateSquares` - Create a list of Squares. Squares are used to track the tokens.
- `NewPlayer` - Creates a player and calls the CreateTokens method. Then sets the correct starting position for the active token.   
- `CreateTokens` - Creates a list of tokens of selected color for the player and assigns the correct route depending on the color choice.
- `CreateRoute(TokenColor color)` and `GetRoute(int delta, int startColor)` - Create a route that depends on tokens color.    

### DataAccess Layer
#### BoardRepo Class
- ``GetAllBoards()`` - Returns all boards from database.
- ``GetBoardByName(string name)`` - Returns the specified board from databas.
- ``UpdateBoard(int id, Board board)`` - Updates board and returns completed task or task from exception.
- ``AddBoard(Board board)`` - Adds board to database and returns completed task or task from exception.
- ``Deleteboard(string name)`` - Finds the board and deletes it from the database, returns completed task or task from exception.
#### GameRepo Class
- ``GetGameByName(string name)`` - Returns the board by name and includes players and tokens, returns a ``board``.
#### PlayerRepo Class
- ``AddPlayer(string playerName, string gameName, string color)`` - Goes through pre-checks to make sure that:
  - The board exists
  - Player count is not already at max
  - The name is not taken 
  - The color has not been taken
- ``GetPlayers(string gameName)`` - returns a list of all players that are related to the board name.
- ``GetPlayerTurn(string gameName)`` - returns the name of the player who has the next turn.
- ``AddPlayerTurnName(string gameName, string playerName)`` - Finds all players related to the board in a list, and selects the person below the included ``playerName`` as the next turn name. Returns task completed or task from exception.
- ``MovePlayer(string gameName, string playerName, int diceNumber)`` - Calls ``Move`` from ``Movement`` class. Move then passes the token through the following methods to determine what actions will be taken:
  - Tododasdas
