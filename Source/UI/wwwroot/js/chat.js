﻿"use strict";

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
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " rolls " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
    UpdateBoard();
});

connection.on("GetPlayerTurn", function(playerName) {
    var isPlayerTurn = playerName.localeCompare(selectedPlayer);
    if (isPlayerTurn == 0) {
        document.getElementById("dice").innerHTML = "";
        document.getElementById("prompt").innerHTML = "Roll the dice";
        document.getElementsByClassName("gameProgressInfo")[0].style.display = 'block';
        document.getElementById("rollDiceButton").disabled = false;
        document.getElementById("moveButton").disabled = true;
        document.getElementById("passMoveButton").disabled = true;
    }
    else {
        document.getElementsByClassName("gameProgressInfo")[0].style.display = 'none';
        document.getElementById("prompt").innerHTML = "Player Turn - " + playerName;
    }
});

connection.start().then(function () { // Start connection
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
    document.getElementById("selectedPlayer").innerHTML = "Player - " + selectedPlayer;
    document.getElementById("userInputRow").style.display = 'none';
    connection.invoke("PlayerTurn", groupName).catch(function(err) {
        return console.error(err.toString());
    });
});

document.getElementById("rollDiceButton").addEventListener("click", async  function (event) { // Get username, and dice API roll when clicking, then join
    diceRoll = await getDice();
    document.getElementById("dice").innerHTML = "Dice: " + diceRoll.toString();
    document.getElementById("prompt").innerHTML = "Choose a token";
    document.getElementById("rollDiceButton").disabled = true;
    document.getElementById("passMoveButton").disabled = false;
    document.getElementsByClassName("game")[0].addEventListener("click", SelectToken);
});

async function getDice() {
    var msg = await fetch('https://localhost/api/dice');
    var data = await msg.json();
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

        if (markedCellId != null) {
            document.getElementById(markedCellId).classList.remove("selectionToken");
        }
        markedCellId = selectedSquare;
        document.getElementById(markedCellId).classList.add("selectionToken");

        document.getElementById(markedCellId).classList.add("selectionToken");
    }
    else {
        document.getElementById("prompt").innerHTML = "Choose a token";
        document.getElementById("moveButton").disable = true;
        if (markedCellId != null) {
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
    }
    else {

        if (markedCellId != null) {
            document.getElementById(markedCellId).classList.remove("selectionToken");
            markedCellId = null;
        }
        document.getElementById("prompt").innerHTML = result + " Choose another token";
        document.getElementById("moveButton").disabled = true;
    }
});
document.getElementById("passMoveButton").addEventListener("click", async function (event) {

        connection.invoke("AddPlayerTurn", groupName, selectedPlayer).catch(function (err) {
            return console.error(err.toString());
        });
        connection.invoke("SendMessage", groupName, selectedPlayer, "passes the move to the next player").catch(function (err) {
            return console.error(err.toString());
        });
        connection.invoke("PlayerTurn", groupName).catch(function (err) {
            return console.error(err.toString());
        });
});
