# Documentation

## TheLudoGame API
### Board Endpoint

| Endpoint           | Method | Description                    | Response codes                  |
| ------------------ | ------ | ------------------------------ | ------------------------------- |
| api/board          | GET    | Returns a list of the boards   | 200 (Success) 204 (No content)  |
| api/board/gameName | GET    | Returns a specific board       | 200 (Success) 404 (Not found)   |
| api/board/id       | PUT    | Updates board                  | 200 (Success) 204 (No content) 400 (Bad Request) 404 (Not found)|
| api/board/gameName | DELETE | Deletes the specified board    | 200 (Success) 404 (Not found)   |
| api/board          | POST   | Adds a board from body request | 200 (Success) 400 (Bad Request) |

### Game Endpoint

| Endpoint              | Method | Description                                                                | Response code                                                   |
|-----------------------|--------|----------------------------------------------------------------------------|------------------------------------------------------------------|
| api/game/gameName   | GET    | Returns a complete game (board + players+tokens) | 200 (Success) 404 (Not found)                                    |

### Players Endpoint
| Endpoint              | Method | Description                                                                | Response codes                                                   |
|-----------------------|--------|----------------------------------------------------------------------------|------------------------------------------------------------------|
| api/players?gameName | POST   | Adds a player to an existing game. Body: player name and tokens color| 200 (Success) 404 (Not found)   |
| api/players/turn/gameName | GET   | Returns the name of the player who has the turn for the selected game | 200 (Success) 400 (Bad Request) |
| api/players/turn/gameName | PUT | Updates the playerturn to the next player in the list, using the game name and the current playername in the request body (automatically figures out the next player)  | 200 (Success) 400 (Bad Request)|
| api/players/gameName | GET   | Gets all players added to the game                                      | 200 (Success) 404 (Not Found)|



### Dice Endpoint
| Endpoint              | Method | Description                                                                | Response codes                                                   |
|-----------------------|--------|----------------------------------------------------------------------------|------------------------------------------------------------------|
| api/dice | GET   | Retuns a random dice number                                          | 200 (Success) |

### Tokens Endpoint   
| Endpoint                      | Method | Description                                                       | Response codes                                          |
|-------------------------------|--------|------------------------------------------------------------------|---------------------------------------------------------|
| api/tokens/tokenId?diceNumber | PUT    | Moves the token {tokenId} by {diceNumber} squares                           |200 (Success) 400 (Bad Request)                          |
 
