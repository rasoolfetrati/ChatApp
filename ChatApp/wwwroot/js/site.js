"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendToUser").disabled = true;

connection.on("ReceiveMessage", function (message) {
    console.log(message);
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var li = `
             <li class="clearfix">
        <div class="message-data">
        <span class="message-data-time">10:12 AM, Today</span>
        </div>
        <div class="message my-message">${msg}</div>
        </li>`;
    $(".messagesList").append(li);
});

connection.start().then(function () {
    connection.invoke("RegisterUser", "amir@yahoo.com").then(function (id) {
        document.getElementById("connectionId").innerHTML = id;
    });
    document.getElementById("sendToUser").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

//document.getElementById("sendButton").addEventListener("click", function (event) {
//    var user = document.getElementById("userInput").value;
//    var message = document.getElementById("messageInput").value;
//    connection.invoke("SendMessage", user, message).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});

document.getElementById("sendToUser").addEventListener("click", function (event) {
    var receiverConnectionId = document.getElementById("receiverId").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendToUser", receiverConnectionId, message).catch(function (err) {
        return console.error(err.toString());
    });
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var li = `
     <li class="clearfix">
                    <div class="message-data text-right">
                        <span class="message-data-time">10:10 AM, Today</span>
                        <img src="https://bootdey.com/img/Content/avatar/avatar7.png" alt="avatar">
                    </div>
                    <div class="message other-message float-right"> ${msg} </div>
     </li>`;
    $(".messagesList").append(li);
    event.preventDefault();
});

document.getElementById("userli").addEventListener("click", function (event) {
    var userfull = document.getElementById("userfullname").innerHTML;
    connection.invoke("GetUserId",userfull).then((userid) => {
        document.getElementById("receiverId").value = userid;
    }).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});