
## Ludo Web

### Pages
- ``Game.cshtml`` - The main page for playing the game. Players connect to a SignalR hub by choosing player name and clicking connect. Contains a HTML drawn Ludo board that updates dynamically each turn via SignalR.
-  ``Game.cshtml.cs`` Back-end of razorpage, calls ``OnGetAsync`` and binds the result to a ```Board``` object.

``OnGetAsync``

  ```csharp
public async Task<ActionResult> OnGetAsync(string boardName) // Get name from url, make API call to find matching board in database.
        {
            //Make API call to get board here
            Board = await _ludoData.GetGameAsync(boardName);
            if (Board == null)
                return RedirectToPage("Index");

            return Page();
        }
```
- ``NewGame.cshtml`` - Uses taghelpers to create a POST form that binds the entered name to a ``Board`` object and sends it to the API.
```html
<form method="post">
    Name:
    <input asp-for="Board.BoardName" required/>
    <input type="submit" class="btn-primary" />
</form>
```
-``NewGame.cshtml.cs`` - Backend makes a post call on form submission and then redirects to the ``Player`` page with the game name as route value.
```csharp
public async Task<IActionResult> OnPostAsync() // After submitting form, create a board and add it to the database if the board with such name does not already exist
        {
            if (!ModelState.IsValid)
                return Page();
            // Search for a game named Board.BoardName in the DB
            var boardInDB = await _ludoData.GetBoardAsync(Board.BoardName);
            
            // if there is no game with this name, create it
            if (boardInDB == null)
            {
                var response = await _ludoData.AddBoard(Board.BoardName);

                if (response.IsSuccessStatusCode)
                    return RedirectToPage("Player", new { gameName = Board.BoardName });
                return Page();
            }
            
            return RedirectToPage("Player", new { gameName = Board.BoardName });
        }
```

