"use strict";

//Baue die Connection auf zu Chathub.
//Wichtig dass /chatHub genauso geschrieben ist wie in der Program.cs registiert
console.log("Verbindung wird aufgebaut");
var connection = new signalR.HubConnectionBuilder().withUrl("/GameHub").build();


connection.onclose(() => {
    connection.start().then(() => {
        console.log("Setze userid.")
        connection.invoke("SetUserIdentifier", userId); // Setze den UserIdentifier
    });
});
//Die Verbindung wurde aufgebaut und ruft nun diese Funktion auf:
connection.start().then(function () {
    console.log("Verbindung erfolgreich.")
}).catch(function (err) {
    console.log("Verbindung fehlgeschlagen");

    //falls die Verbindung fehlschlägt. man könnte z.b. die seite neu laden lassen.
    return console.error(err.toString());

});

connection.on("SetCardImage", function (args) {

    var image = document.getElementById("Spieler1_Card_"+args[0]);
    image.src = "/images/card/" + args[1] +".png";

});

connection.on("SetCurrentPlayer", function (args) {

    Console.log(args[0]);

});


document.getElementById("startButton").addEventListener("click", function (event) {

        console.log("Game wird gestartet");
        connection
            .invoke("StartGame")
            .catch(function (err) {
                return console.error(err.toString());
            });
    });
document.getElementById("hitButton").addEventListener("click", function (event) {

    console.log("Spieler drückt hitButton");
    connection
        .invoke("hit")
        .catch(function (err) {
            return console.error(err.toString());
        });
});
