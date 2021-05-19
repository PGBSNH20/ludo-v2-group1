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
| api/players?gameName | Post   | Adds a player to an existing game                                          | 200 (Success) 404 (Not found) (Exceeded the number of players)   |
