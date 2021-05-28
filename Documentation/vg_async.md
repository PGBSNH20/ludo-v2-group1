
## SignalR Implementation i Ludo
Vi har använt oss av SignalR för att utföra klient-server anrop och uppdatera sidan asynkront.

Våra signaler går genom ``GameHub``. Följande metoder har implementerats:

### Send Message
Metoden skickar meddelandet som innehåller användare och meddelande till alla klienter i en grupp, gruppens namn är samma som spelets namn, javascriptet tar spelnamnet och skickar det till metoden. Metoden skickar sedan ut till ``RecieveMessage`` som är en javascript funktion som skriver ut info och uppdaterar även brädan. Metoden triggar vid dice roll samt när användaren trycker på knappar ``Move token`` och ``Pass move``.
```csharp
public async Task SendMessage(string group, string user, string message)
        {
            await Clients.Group(group).SendAsync("ReceiveMessage", user, message);
        }
```
##### Javascript
```JavaScript
connection.on("ReceiveMessage", function (user, message) { // A connection on SingalR method SendMessage returns a string in the message list html and updates board
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " rolls " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
    UpdateBoard();
});
```

### Join Group
Metoden triggas när spelaren ansluter till spelet genom att välja namn och sedan klickar "Connect to game". Gruppens namn är samma som spelets namn, javascriptet tar spelnamnet och skickar det till metoden.

```csharp
public async Task JoinGroup(string group)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
        }
```
##### Javascript (Relevant del)
```JavaScript
document.getElementById("userSubmit").addEventListener("click", function(event) {
    connection.invoke("JoinGroup", groupName).catch(function (err) {
        return console.error(err.toString());
    });
```
### PlayerTurn
Metoden hämtar namnet på vems tur det är att spela, namnet är sparat i databasen för brädan.
```csharp
public async Task PlayerTurn(string group)
        {
            string playerName = await _ludoData.GetPlayerTurn(group);
            await Clients.Group(group).SendAsync("GetPlayerTurn", playerName);
        }
```
##### API Anrop
```csharp
public async Task<string> GetPlayerTurn(string gameName)
        {
            string msg = string.Empty;
            HttpResponseMessage response = await _client.GetAsync($"https://localhost/api/players/turn/{gameName}");

            if (response.IsSuccessStatusCode)
                msg = await response.Content.ReadAsStringAsync();
            return msg;
        }
```
##### Javascript
Javascript jämför namnet på spelaren tur med klientens namn, om det är samma så enablas dice knappen.
```JavaScript
connection.on("GetPlayerTurn", function(playerName) {
    document.getElementById("playerTurn").innerHTML = "Player Turn - " + playerName;
    document.getElementById("sendButton").disabled = true;

    var isPlayerTurn = playerName.localeCompare(selectedPlayer);
    if(isPlayerTurn == 0)
        document.getElementById("sendButton").disabled = false;
});
```
### Add Player Turn
Metoden uppdaterar vilken spelares tur det är, genom att skicka spelnamnet och klientens namn till APIt. Servern listar sedan ut vems tur det är näst och uppdaterar databasen.
```csharp
public async Task AddPlayerTurn(string group, string name)
        {
            await _ludoData.AddPlayerTurn(group, name);
            await Clients.Group(group).SendAsync("GetPlayerTurn", name);
        }
```
##### API Anrop
```csharp
public async Task<HttpResponseMessage> AddPlayerTurn(string gameName, string playerName)
        {
            var json = JsonConvert.SerializeObject(playerName);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            return await _client.PutAsync($"https://localhost/api/players/turn/{gameName}", data);
        }
```
##### Javascript
Triggas vid dice roll
```Javascript

    connection.invoke("AddPlayerTurn", groupName, selectedPlayer).catch(function (err) {
        return console.error(err.toString());
    });
```
