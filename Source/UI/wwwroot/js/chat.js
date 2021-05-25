"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();
var groupName = document.getElementById("groupName").innerHTML; // Set the group name by getting the name of the board.
//var playerTurn = null;
//var playerList = null;
//var playerTurnId = 0;
var diceRoll = 0;
var oldTokens = []; // array with squareID where tokens are placed
var board;
var selectedPlayer = null;
var selectedToken = null;
var colors = ["b", "y", "r", "g"];

//Disable rollDice button until connection is established
document.getElementById("rollDiceButton").disabled = true; 

connection.on("ReceiveMessage", function (user, message) { // A connection on SingalR method SendMessage returns a string in the message list html
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " rolls " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
    UpdateBoard();
});

connection.on("GetPlayerTurn", function(playerName) {
    document.getElementById("playerTurn").innerHTML = "Player Turn - " + playerName;
    document.getElementById("rollDiceButton").disabled = true;

    var isPlayerTurn = playerName.localeCompare(selectedPlayer);
    if (isPlayerTurn == 0) {
        document.getElementById("prompt").style.display = 'block';
        document.getElementById("prompt").innerHTML = "Roll the dice";
        document.getElementById("rollDiceButton").style.display = 'block';
        document.getElementById("rollDiceButton").disabled = false;
    }
});

connection.start().then(function () { // Start connection and enable dice button
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("userSubmit").addEventListener("click", function(event) {
    connection.invoke("JoinGroup", groupName).catch(function (err) {
        return console.error(err.toString());
    });
    var radios = document.querySelectorAll('input[type="radio"]');

    for (var i = 0; i < radios.length; i++) {
        if (radios[i].checked) {
            selectedPlayer=radios[i].value;
        }
    }
    
    document.getElementById("selectedPlayer").innerHTML = selectedPlayer;
    document.getElementById("selectedPlayer").style.display = 'block';

    var x = document.getElementById("userInputRow");
    if (x.style.display === "none") {
        x.style.display = "block";
    } else {
        x.style.display = "none";
    }
    connection.invoke("PlayerTurn", groupName).catch(function(err) {
        return console.error(err.toString());
    });

});

document.getElementById("rollDiceButton").addEventListener("click", async  function (event) { // Get username, and dice API roll when clicking, then join
    var s = await getDice();
    document.getElementById("dice").innerHTML = "Dice: " + s.toString();
    document.getElementById("dice").style.display = 'block';
    document.getElementById("prompt").innerHTML = "Choose a token";
    document.getElementById("rollDiceButton").disabled = true;
    document.getElementById("moveButton").disabled = true;
    document.getElementById("moveButton").style.display = 'block';
    document.getElementsByClassName("game")[0].addEventListener("click", SelectToken);

    event.preventDefault();
});

async function getDice() {
    var msg = await fetch('https://localhost/api/dice');
    var data = await msg.json();
    diceRoll = data;
    return data;
}

function sendDice(dice, tokenID) {
    console.log("dice:" + dice + "  tokenID:" + tokenID);
    return "placeholder"; 
//    //PUT request with body equal on data in JSON format
//    fetch('https://localhost/api/players/dice/'+tokenID+'?diceNumber='+dice, {
//            method: 'PUT'
//        })
//        .then((response) => response.json())
////Then with the data from the response in JSON...
//        .then((data) => {
//            console.log('Success:', data);
//        })
////Then with the error genereted...
//        .catch((error) => {
//            console.error('Error:', error);
//        });
}

window.onload = UpdateBoard();

async function UpdateBoard() {
    // remove all tokens from the board if they are there
    if (oldTokens.length != 0) {
        for (var i = 0; i < oldTokens.length; i++) {
            document.getElementById(oldTokens[i]).classList.remove("b-token", "g-token", "r-token", "y-token",
                                                                    "b-block", "g-block", "r-block", "y-block");
        }
        oldTokens = []; 
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
                id = token.squareID;
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

function SelectToken(e) {
   
    var selectedSquare = e.target.id;
    var squaresWithPlayersTokens = GetSquaresWithPlayersTokens();
   
    var indexOfSelectedToken = squaresWithPlayersTokens.indexOf(selectedSquare);
    if (indexOfSelectedToken > -1) {
        document.getElementById("prompt").innerHTML = "You have chosen a token on the square " + selectedSquare;
        selectedToken = board.players.find(p => p.name == selectedPlayer).tokens[indexOfSelectedToken];
        document.getElementById("moveButton").disabled = false;
    }
    else {
        document.getElementById("prompt").innerHTML = "Choose a token";
        document.getElementById("moveButton").disable = true;
    }
}

function GetSquaresWithPlayersTokens() {
    var player = board.players.find(p => p.name == selectedPlayer);
    var tokens = player.tokens;
    var squares = [];
    var color = tokens[0].color;

    for (var j = 0; j < tokens.length; j++) {
        if (tokens[j].isActive) {
            squares.push(tokens[j].squareID.toString());
        }
        else {
            squares.push(colors[color] + j);
        }
    }
    return squares;
}

document.getElementById("moveButton").addEventListener("click", async function (event) {

    var result = sendDice(diceRoll, selectedToken.id);

    if (result == "placeholder") {
        connection.invoke("AddPlayerTurn", groupName, selectedPlayer).catch(function (err) {
            return console.error(err.toString());
        });
        connection.invoke("SendMessage", groupName, selectedPlayer, diceRoll.toString()).catch(function (err) {
            return console.error(err.toString());
        });

        connection.invoke("PlayerTurn", groupName).catch(function (err) {
            return console.error(err.toString());
        });
        document.getElementById("moveButton").style.display = 'none';
        document.getElementById("prompt").style.display = 'none';
        document.getElementById("rollDiceButton").style.display = 'none';
        document.getElementById("dice").style.display = 'none';
       // event.preventDefault();
    }
    else {
        document.getElementById("prompt").innerHTML = result + " Choose another token";
    }
    document.getElementById("moveButton").disabled = true;
});
