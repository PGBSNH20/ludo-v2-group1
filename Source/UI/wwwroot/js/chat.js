"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();
var groupName = document.getElementById("groupName").innerHTML; // Set the group name by getting the name of the board.
var playerTurn = null;
var playerList = null;
var playerTurnId = 0;
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
    var user = document.getElementById("userInput").value;

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
    var x = document.getElementById("userInputRow");
    if (x.style.display === "none") {
        x.style.display = "block";
    } else {
        x.style.display = "none";
    }
    
    var user = document.getElementById("userInput").value;
    if(user == playerTurn)
        document.getElementById("sendButton").disabled = false;

    connection.invoke("PlayerTurn", groupName).catch(function(err) {
        return console.error(err.toString());
    });

});

document.getElementById("sendButton").addEventListener("click", async  function (event) { // Get username, and dice API roll when clicking, then join
    var user = document.getElementById("userInput").value;
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