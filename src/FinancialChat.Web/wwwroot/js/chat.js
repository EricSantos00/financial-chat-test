"use strict";

const connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

// Disable the join button until connection is established.
document.getElementById("joinRoomButton").disabled = true;

//Disable the send button until user joins a room.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (date, user, message) {
    const li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);

    const listCount = document.getElementById("messagesList").childElementCount;

    if (listCount > 50) {
        document.getElementById("messagesList").removeChild(document.getElementById("messagesList").firstChild);
    }

    const formattedDate = new Date(date).toLocaleString();

    if (user === "[System]") {
        li.innerHTML = `<b>[${formattedDate}] ${user}: ${message}</b>`;
    } else {
        li.innerHTML = `<b>[${formattedDate}] ${user}:</b> ${message}`;
    }
});

connection.on("JoinedRoom", function () {
    document.getElementById("messagesList").innerHTML = '';
});

connection.start().then(function () {
    document.getElementById("joinRoomButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    const message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", message)
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
        .catch(function (err) {
            return console.error(err.toString());
        });

    document.getElementById("sendButton").disabled = false;

    event.preventDefault();
});