﻿"use strict";

//Baue die Connection auf zu Chathub.
//Wichtig dass /chatHub genauso geschrieben ist wie in der Program.cs registiert
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
console.log("test1");

//Die Verbindung wurde aufgebaut und ruft nun diese Funktion auf:
connection.start().then(function () {

    //weiteren Skriptcode zur Veränderung des Frontends.
    document.getElementById("sendButton").disabled = false;
    connection
        .invoke("onConnection")
        .catch(function (err) {
            return console.error(err.toString());
        });

}).catch(function (err) {

    //falls die Verbindung fehlschlägt. man könnte z.b. die seite neu laden lassen.
    return console.error(err.toString());

});



// Ein Event welches aus dem Backend aufgerufen wird:
//Der Name muss genauso im Backend geschrieben sein!
connection.on("ReceiveMessage", function (user, message) {
    try {
        sendMessage(user, message);
    }
    catch (err) {
        console.log("ERROR " + err.message);
    }
});

function sendMessage(user, message) {
    console.log("test2");

    console.log("test4" + message);

    //javascript Code der dann passieren soll
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `${user} sayed ${message}`;
}


connection.on("console", function (message) {
    console.log(message);
});

//Ein Javascriptevent welches Informationen an das Backend schickt.
document.getElementById("sendButton")
    //Die Art des Events, da gibt es noch mehr verschiedene
    .addEventListener("click", function (event) {

        //Standardweitergabe von Paramentern
        //Man kann bestimmt noch mehr weitergeben
        var user = document.getElementById("userInput").value;
        var message = document.getElementById("messageInput").value;

        //Aufruf der Backendmethode
        connection
            //Name muss wieder gleich wie die Methode im zugehörigen Hub sein
            .invoke("SendMessage", user, message)
            //falls fehler passieren
            .catch(function (err) {
                return console.error(err.toString());
            });
        event.preventDefault();
});