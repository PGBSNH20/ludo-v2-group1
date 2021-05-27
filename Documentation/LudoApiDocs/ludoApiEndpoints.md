# Documentation

## TheLudoGame API
### Game Endpoint
| Endpoint              | Method | Description                                                                | Response codes                                                   |
|-----------------------|--------|----------------------------------------------------------------------------|------------------------------------------------------------------|
| api/game             | GET    | Returns a list of the names of all saved games                             | 200 (Success) 204 (No content)                                   |
| api/game/gameName   | GET    | Returns a saved game                                                       | 200 (Success) 404 (Not found)                                    |
| api/game/gameName   | PUT    | Creates a new game or updates an existing game                             | 200 (Success)                                                    |
| api/game/gameName   | DELETE | Deletes the specified game                                                 | 200 (Success) 404 (Not found)                                    |

### Player Endpoint
| Endpoint              | Method | Description                                                                | Response codes                                                   |
|-----------------------|--------|----------------------------------------------------------------------------|------------------------------------------------------------------|
| api/players?gameName | POST   | Adds a player to an existing game                                          | 200 (Success) 404 (Not found) (Exceeded the number of players)   |
| api/players/turn/gameName | GET   | Retruns the name of the player who has the turn for the selected game                                         | 200 (Success)|
| api/players/turn/gameName | PUT | Updates the playerturn to the next player in the list, using the game name and the current playername in the request body (automatically figures out the next player)  | 200 (Success) ADD BAD REQUEST| 
| api/players/gameName | GET   | Gets all players added to the game                                      | 200 (Success) 404 (Not Found)|


### Dice Endpoint
| Endpoint              | Method | Description                                                                | Response codes                                                   |
|-----------------------|--------|----------------------------------------------------------------------------|------------------------------------------------------------------|
| api/dice | GET   | Retuns a random dice number                                          | 200 (Success) |

