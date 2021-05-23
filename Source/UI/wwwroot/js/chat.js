"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();
var groupName = document.getElementById("groupName").innerHTML; // Set the group name by getting the name of the board.
var playerTurn = null;
var playerList = null;
var playerTurnId = 0;
var diceRoll = 0;
var oldTokens = []; // array with squareID where tokens are placed
var board;

var colors = ["b", "y", "r", "g"];

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true; 

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
    var user = document.getElementById("selectedPlayer").innerHTML;

    document.getElementById("sendButton").disabled = true;
    var isPlayerTurn = playerName.localeCompare(user);
    if(isPlayerTurn == 0)
        document.getElementById("sendButton").disabled = false;
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
    var selection;
    for (var i = 0; i < radios.length; i++) {
        if (radios[i].checked) {
            selection=radios[i].value;
        }
    }

    document.getElementById("selectedPlayer").innerHTML = selection;
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

document.getElementById("sendButton").addEventListener("click", async  function (event) { // Get username, and dice API roll when clicking, then join
    var user = document.getElementById("selectedPlayer").innerHTML;
    var s = await getDice();
    var message = s.toString();
    
    connection.invoke("AddPlayerTurn", groupName, user).catch(function (err) {
        return console.error(err.toString());
    });

    document.getElementById("sendButton").disabled = true;
    connection.invoke("SendMessage", groupName, user, message).catch(function (err) {
        return console.error(err.toString());
    });

    connection.invoke("PlayerTurn", groupName).catch(function(err) {
        return console.error(err.toString());
    });
    
    event.preventDefault();
});


async function getDice() {
    var msg = await fetch('https://localhost/api/dice');
    var data = await msg.json();
    diceRoll = data;
    var user = document.getElementById("selectedPlayer").innerHTML;
    sendDice(user, diceRoll);
    return data;
}

async function sendDice(playerName, dice) {

    //PUT request with body equal on data in JSON format
    fetch('https://localhost/api/players/dice/' + groupName + '?diceNumber=' + dice, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(playerName)
        })
        .then((response) => response.json())
//Then with the data from the response in JSON...
        .then((data) => {
            console.log('Success:', data);
        })
//Then with the error genereted...
        .catch((error) => {
            console.error('Error:', error);
        });
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