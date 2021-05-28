"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();
var groupName = document.getElementById("groupName").innerHTML; // Set the group name by getting the name of the board.
var diceRoll = null;
var oldTokens = []; // array with squareID where tokens are placed
var board;
var selectedPlayer = null;
var selectedToken = null;
var markedCellId = null;
var colors = ["b", "y", "r", "g"];

window.onload = UpdateBoard();
connection.on("ReceiveMessage", function (user, message) { // A connection on SingalR method SendMessage returns a string in the message list html
    // creates a message with game progress information and adds it to the messagesList
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + msg;
    var li = document.createElement("li");
    var br = document.createElement("br");
    var timeNow = new Date();
    li.textContent = timeNow.getHours() + ":" + timeNow.getMinutes() + ":" + timeNow.getSeconds() +"  " + encodedMsg;
    var messagesList = document.getElementById("messagesList");
    var latestMessage = messagesList.firstChild;
    messagesList.insertBefore(li, latestMessage);
    document.getElementById("messagesList").appendChild(br);

    // if user won
    if (msg.indexOf("Win") > -1) {
        document.getElementById("prompt").innerHTML = encodedMsg;
        document.getElementsByClassName("gameProgressInfo")[0].style.display = 'block';
        document.getElementById("rollDiceButton").style.display = 'none';
        document.getElementById("moveButton").style.display = 'none';
        document.getElementById("passMoveButton").style.display = 'none';
        document.getElementById("newGameButton").style.display = 'block';
    }
    // if it's not a win yet 
    else {
        UpdateBoard();
    }
});

connection.on("GetPlayerTurn", function(playerName) {
    var isPlayerTurn = playerName.localeCompare(selectedPlayer);
    // if it is users turn to move
    if (isPlayerTurn == 0) {
        document.getElementById("dice").innerHTML = "";
        document.getElementById("prompt").innerHTML = "Roll the dice";
        document.getElementsByClassName("gameProgressInfo")[0].style.display = 'block';
        document.getElementById("rollDiceButton").disabled = false;
        document.getElementById("moveButton").disabled = true;
        document.getElementById("passMoveButton").disabled = true;
    }
    // if its somepne else turn to move
    else {
        document.getElementsByClassName("game")[0].removeEventListener("click", SelectToken);
        document.getElementsByClassName("gameProgressInfo")[0].style.display = 'none';
        document.getElementById("prompt").innerHTML = "Player Turn - " + playerName;
    }
});

connection.start().then(function () { // Start connection
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("userSubmit").addEventListener("click", function(event) { //user selectes the player and joins the chat group
    connection.invoke("JoinGroup", groupName).catch(function (err) {
        return console.error(err.toString());
    });
    var radios = document.querySelectorAll('input[type="radio"]');

    for (var i = 0; i < radios.length; i++) {
        if (radios[i].checked) {
            selectedPlayer=radios[i].value;
        }
    }
    document.getElementById("selectedPlayer").innerHTML = "Player - " + selectedPlayer;
    document.getElementById("userInputRow").style.display = 'none';
    connection.invoke("PlayerTurn", groupName).catch(function(err) {
        return console.error(err.toString());
    });
});

document.getElementById("rollDiceButton").addEventListener("click", async  function (event) {
    // calls the method that make API request and get diceNumber 
    diceRoll = await getDice();
    document.getElementById("dice").innerHTML = "Dice: " + diceRoll.toString();
    document.getElementById("prompt").innerHTML = "Choose a token";
    document.getElementById("rollDiceButton").disabled = true;
    document.getElementById("passMoveButton").disabled = false;
    document.getElementsByClassName("game")[0].addEventListener("click", SelectToken);
    connection.invoke("SendMessage", groupName, selectedPlayer, " rolls "+diceRoll.toString()).catch(function (err) {
        return console.error(err.toString());
    });
});

async function getDice() {
    var msg = await fetch('https://localhost/api/dice');
    var data = await msg.json();
    return data;
}

async function sendDice(dice, tokenID) {

    try {
        var movementResult = await fetch("https://localhost/api/tokens/" + tokenID + "?diceNumber=" + dice, { "method": "PUT" });
        var json = await movementResult.json();
        return json;
    }
    catch (error) {
        console.log(error);
    }
}

// Get informations from API about position of tokens and places tokens on the board
async function UpdateBoard() {
    // remove all tokens from the board if they are there
    if (oldTokens.length != 0) {
        for (var i = 0; i < oldTokens.length; i++) {
            document.getElementById(oldTokens[i]).classList.remove("b-token", "g-token", "r-token", "y-token",
                                                                    "b-block", "g-block", "r-block", "y-block");
        }
        oldTokens = []; 
    }
    if (markedCellId != null) {
        document.getElementById(markedCellId).classList.remove("selectionToken");
    }
    // get informations from API about position of tokens 
    var boardAnswer = await fetch('https://localhost/api/Game/' + groupName);
    board = await boardAnswer.json();
    var id;
    // place tokens on the board
    for (var i = 0; i < board.players.length; i++)
    {
        for (var j = 0; j < board.players[i].tokens.length; j++) {
            var token = board.players[i].tokens[j];
            var color = token.color;
            var colorTokenClass = colors[color] + '-token';
            if (token.isActive) {
                id = token.squareId;
                if (document.getElementById(id).classList.contains(colorTokenClass)) {
                    colorTokenClass = colors[color] + '-block';
                    document.getElementById(id).classList.add(colorTokenClass);
                }
                else {
                    document.getElementById(id).classList.add(colorTokenClass);
                }
            }
            else {
                id = colors[color] + j;
                document.getElementById(id).classList.add(colorTokenClass);
            } 
            oldTokens.push(id); // save squareIDs where tokens are placed
        }
    }
}

// Mark the token selected by the user
function SelectToken(e) {
   
    var selectedSquare = e.target.id; // SquareId that the user clicked on
    var squaresWithPlayersTokens = GetSquaresWithPlayersTokens();
   
    var indexOfSelectedToken = squaresWithPlayersTokens.indexOf(selectedSquare);
    // if user clicked on her token 
    if (indexOfSelectedToken > -1) {
        document.getElementById("prompt").innerHTML = "You have chosen a token on the square " + selectedSquare;
        selectedToken = board.players.find(p => p.name == selectedPlayer).tokens[indexOfSelectedToken];
        document.getElementById("moveButton").disabled = false;

        if (markedCellId != null) { //removes the previous selection
            document.getElementById(markedCellId).classList.remove("selectionToken");
        }
        markedCellId = selectedSquare;
        document.getElementById(markedCellId).classList.add("selectionToken");
    }
    //if user clicked on an empty square or someone else's token 
    else {
        document.getElementById("prompt").innerHTML = "Choose a token";
        document.getElementById("moveButton").disable = true;
        if (markedCellId != null) { //removes the previous selection
            document.getElementById(markedCellId).classList.remove("selectionToken");
            markedCellId = null;
        }
    }
}

function GetSquaresWithPlayersTokens() {
    var player = board.players.find(p => p.name == selectedPlayer);
    var tokens = player.tokens;
    var squares = [];
    var color = tokens[0].color;

    for (var j = 0; j < tokens.length; j++) {
        if (tokens[j].isActive) {
            squares.push(tokens[j].squareId.toString());
        }
        else {
            squares.push(colors[color] + j);
        }
    }
    return squares;
}

document.getElementById("moveButton").addEventListener("click", async function (event) {
    // calls the method that makes API request and moves selected token
    var movementResult = await sendDice(diceRoll, selectedToken.id);

    // if user won 
    if (movementResult.indexOf("Win") > -1) {
        // Deletes the game from the DB 
        try {
             fetch("https://localhost/api/board/" + board.boardName, { "method": "DELETE" });
        }
        catch (error) {
            console.log(error);
        }
        connection.invoke("SendMessage", groupName, selectedPlayer, ": " + movementResult).catch(function (err) {
            return console.error(err.toString());
        });
    }
     // if user can move selected token but it's not a win yet
    else if (movementResult.indexOf("You made a move!") > -1 || movementResult.indexOf("Token at the finish!")>-1) {
        connection.invoke("AddPlayerTurn", groupName, selectedPlayer).catch(function (err) {
            return console.error(err.toString());
        });
        connection.invoke("SendMessage", groupName, selectedPlayer, ": " + movementResult).catch(function (err) {
            return console.error(err.toString());
        });
        connection.invoke("PlayerTurn", groupName).catch(function (err) {
            return console.error(err.toString());
        });
    }
    // if user can't move selected token
    else {

        if (markedCellId != null) { //removes selection from the token
            document.getElementById(markedCellId).classList.remove("selectionToken");
            markedCellId = null;
        }
        document.getElementById("prompt").innerHTML = movementResult + " Choose another token";
        document.getElementById("moveButton").disabled = true;
    }
});

document.getElementById("passMoveButton").addEventListener("click", async function (event) {

        connection.invoke("AddPlayerTurn", groupName, selectedPlayer).catch(function (err) {
            return console.error(err.toString());
        });
        connection.invoke("SendMessage", groupName, selectedPlayer, " passed the move to the next player").catch(function (err) {
            return console.error(err.toString());
        });
        connection.invoke("PlayerTurn", groupName).catch(function (err) {
            return console.error(err.toString());
        });
});
