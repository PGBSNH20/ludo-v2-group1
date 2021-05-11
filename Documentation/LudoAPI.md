# Documentation

## TheLudoGame API
### Game Endpoint
| Endpoint              | Method | Description                                                                | Response codes                                                   |
|-----------------------|--------|----------------------------------------------------------------------------|------------------------------------------------------------------|
| api/games             | GET    | Returns a list of the names of all saved games                             | 200 (Success) 204 (No content)                                   |
| api/games/game name   | GET    | Returns a saved game                                                       | 200 (Success) 404 (Not found)                                    |
| api/games/game name   | PUT    | Creates a new game or updates an existing game                             | 200 (Success)                                                    |
| api/games/game name   | DELETE | Deletes the specified game                                                 | 200 (Success) 404 (Not found)                                    |

### Player Endpoint
| Endpoint              | Method | Description                                                                | Response codes                                                   |
|-----------------------|--------|----------------------------------------------------------------------------|------------------------------------------------------------------|
| api/players?game name | Post   | Adds a player to an existing game                                          | 200 (Success) 404 (Not found) (Exceeded the number of players)   |
