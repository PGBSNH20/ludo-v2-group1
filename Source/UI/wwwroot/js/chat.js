"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();
var groupName = document.getElementById("groupName").innerHTML; // Set the group name by getting the name of the board.

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true; 


connection.on("ReceiveMessage", function (user, message) { // A connection on SingalR method SendMessage returns a string in the message list html
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " rolls " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
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
    document.getElementById("sendButton").disabled = false;
});


document.getElementById("sendButton").addEventListener("click", async  function (event) { // Get username, and dice API roll when clicking, then join
    var user = document.getElementById("selectedPlayer").innerHTML;
    var s = await get();
    var message = s.toString();

    connection.invoke("SendMessage",groupName, user, message).catch(function (err) {
        return console.error(err.toString());
    });

    event.preventDefault();
});


async function get() {
    var msg = await fetch('https://localhost/api/dice');
    var data = msg.json();
    console.log(await data);
    return await data;
}