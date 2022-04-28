"use strict";

const connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

// Disable the join button until connection is established.
document.getElementById("joinRoomButton").disabled = true;

//Disable the send button until user joins a room.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    const li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
});

connection.start().then(function () {
    document.getElementById("joinRoomButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    const user = document.getElementById("userInput").value;
    const message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message)
        .catch(function (err) {
            return console.error(err.toString());
        });
    event.preventDefault();
});

document.getElementById("joinRoomButton").addEventListener("click", function (event) {
    // Disable the sendButton to prevent the user to send message if the join fails.
    document.getElementById("sendButton").disabled = true;
    
    const room = document.getElementById("roomNameInput").value;
    
    connection.invoke("JoinRoom", room)
        .then(function () {
            document.getElementById("sendButton").disabled = false;
        })
        .catch(function (err) {
        return console.error(err.toString());
    });

    event.preventDefault();
});