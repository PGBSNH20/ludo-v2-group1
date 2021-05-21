"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();
var groupName = document.getElementById("groupName").innerHTML; // Set the group name by getting the name of the board.
var playerTurn = null;
var playerList = null;
var playerTurnId = 0;

var colors = ["b", "y", "r", "g"];

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true; 

connection.on("ReceiveMessage", function (user, message) { // A connection on SingalR method SendMessage returns a string in the message list html
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " rolls " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.on("GetPlayerTurn", function(playerName) {
    document.getElementById("playerTurn").innerHTML = "Player Turn - " + playerName;
    var user = document.getElementById("selectedPlayer").innerHTML;

    document.getElementById("sendButton").disabled = true;
    var n = playerName.localeCompare(user);
    console.log(n);
    if(n == 0)
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
    
    //var user = document.getElementById("userInput").value;
    //if(user == playerTurn)
    //    document.getElementById("sendButton").disabled = false;

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
    return data;
}

window.onload = UpdateBoard();

async function UpdateBoard() {
    var boardAnswer = await fetch('https://localhost/api/Game/' + groupName);
    var board = await boardAnswer.json();

    console.log(board);
    for (var i = 0; i < board.players.length; i++)
    {
        for (var j = 0; j < board.players[i].tokens.length; j++) {
            var token = board.players[i].tokens[j];
            var color = token.color;
            var colorTokenClass = colors[color] + '-token';
            if (token.isActive) {
                var id = token.squareID;
                if (document.getElementById(id).classList.contains(colorTokenClass)) {
                    colorTokenClass = colors[color] + '-block';
                    document.getElementById(id).classList.add(colorTokenClass);
                }
                else {
                    document.getElementById(id).classList.add(colorTokenClass);
                }
            }
            else {
                var id = colors[color] + j;
                document.getElementById(id).classList.add(colorTokenClass);
            } 
        }
    }
}