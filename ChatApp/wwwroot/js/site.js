"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendToUser").disabled = true;

connection.on("ReceiveMessage", function (messageServer, date, name) {
    var msg = messageServer.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var li = `
        <div class="chat-message-left pb-4">
                        <div>
                            <img src="https://bootdey.com/img/Content/avatar/avatar3.png" class="rounded-circle mr-1" alt="${name}" width="40" height="40">
                            <div class="text-muted small text-nowrap mt-2">${date}</div>
                        </div>
                        <div class="flex-shrink-1 bg-light rounded py-2 px-3 ml-3">
                            <div class="font-weight-bold mb-1">${name}</div>
                            ${msg}
                        </div>
          </div>`;
    $(".messagesList").append(li);
});

connection.start().then(function () {
    connection.invoke("GetCurrentUserId").then(function (id) {
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
    if (receiverConnectionId === null || receiverConnectionId === "") {
        alert("select a user!");
    } else {
        if (message !== null && message !== "") {
            connection.invoke("SendToUser", receiverConnectionId, message).catch(function (err) {
                return console.error(err.toString());
            });
            var today = new Date();
            var dd = String(today.getDate()).padStart(2, '0');
            var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
            var yyyy = today.getFullYear();

            today = mm + '/' + dd + '/' + yyyy;
            var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
            var li = `
              <div class="chat-message-right mb-4">
                        <div>
                            <img src="https://bootdey.com/img/Content/avatar/avatar1.png" class="rounded-circle mr-1" alt="Chris Wood" width="40" height="40">
                            <div class="text-muted small text-nowrap mt-2">${today}</div>
                        </div>
                        <div class="flex-shrink-1 bg-light rounded py-2 px-3 mr-3">
                            <div class="font-weight-bold mb-1">You</div>
                            ${msg}
                        </div>
               </div>`;
            $(".messagesList").append(li);
        } else {
            alert("Fill input");
        }
    }


    document.getElementById("messageInput").value = "";
    document.getElementById("messageInput").focus();
    event.preventDefault();
});


// Get all the div elements with class 'userClass'
const liElements = document.querySelectorAll('div.userClass');

// Add a click event listener to each div element
liElements.forEach((div, index) => {
    // Set a unique data-id attribute for each div element
    div.setAttribute('data-id', index);

    div.addEventListener('click', (e) => {

        // Remove the "current" class from any previously clicked div element
        const currentLi = document.querySelector('.active');
        if (currentLi) {
            currentLi.classList.remove('active');
        }
        // Add the "current" class to the clicked div element
        div.classList.add('active');
        // Get the <p> element inside the clicked div element
        const pElement = div.querySelector('span');

        // Get the value of the id attribute of the <span> element
        const pId = pElement.getAttribute('id');
        connection.invoke("GetUserId", pId).then((userid) => {
            document.getElementById("receiverId").value = userid;
        }).then(() => {
            connection.invoke("GetUserMessages", pId).then((listMessages) => {
                document.getElementById("chathistory").innerHTML = "";
                var li = ``;
                listMessages.forEach((item) => {
                    console.log(item);
                    if (item.isCurrentUser) {
                        li = `
                            <div class="chat-message-right mb-4">
                                <div>
                                    <img src="https://bootdey.com/img/Content/avatar/avatar1.png" class="rounded-circle mr-1" alt="${item.userId}" width="40" height="40">
                                    <div class="text-muted small text-nowrap mt-2">${item.sendDate}</div>
                                </div>
                                <div class="flex-shrink-1 bg-light rounded py-2 px-3 mr-3">
                                    <div class="font-weight-bold mb-1">${item.userId}</div>
                                    ${item.text}
                                </div>
                            </div>`;
                    } else {
                        li = `
                            <div class="chat-message-left pb-4">
                                <div>
                                    <img src="https://bootdey.com/img/Content/avatar/avatar3.png" class="rounded-circle mr-1" alt="${item.userId}" width="40" height="40">
                                    <div class="text-muted small text-nowrap mt-2">${item.sendDate}</div>
                                </div>
                                <div class="flex-shrink-1 bg-light rounded py-2 px-3 ml-3">
                                    <div class="font-weight-bold mb-1">${item.userId}</div>
                                    ${item.text}
                                </div>
                            </div>`;
                    }
                    $(".messagesList").append(li);
                });

            });
        }).catch(function (err) {
            return console.error(err.toString());
        });

        e.preventDefault();
    });
});