"use strict";

//Baue die Connection auf zu Chathub.
//Wichtig dass /chatHub genauso geschrieben ist wie in der Program.cs registiert
console.log("Verbindung wird aufgebaut");
var connection = new signalR.HubConnectionBuilder().withUrl("/GameHub").build();


//Die Verbindung wurde aufgebaut und ruft nun diese Funktion auf:
connection.start().then(function () {
    console.log("Verbindung wurde erfolgreich aufgebaut");

    connection
        //Name muss wieder gleich wie die Methode im zugehörigen Hub sein
        .invoke("StartGame")
        //falls fehler passieren
        .catch(function (err) {
            return console.error(err.toString());
        });

}).catch(function (err) {
    console.log("Verbindung fehlgeschlagen");

    //falls die Verbindung fehlschlägt. man könnte z.b. die seite neu laden lassen.
    return console.error(err.toString());

});



// Ein Event welches aus dem Backend aufgerufen wird:
//Der Name muss genauso im Backend geschrieben sein!
connection.on("SetCardImage", function (args) {

    //javascript Code der dann passieren soll
    var image = document.getElementById("Spieler1_Card_"+args[0]);
    image.src = "/images/card/" + args[1] +".png";


});
