﻿
connection.on("SetCardImage", function (args) {

    Console.log("test");
    var image = document.getElementById("Spieler1_Card_"+args[0]);
    image.src = "/images/card/" + args[1] +".png";

});


connection.on("SetCurrentPlayer", function (args) {

    Console.log(args[0]);

});


document.getElementById("startButton").addEventListener("click", function (event) {

        console.log("Game wird gestartet");
        connection
            .invoke("start")
            .catch(function (err) {
                return console.error(err.toString());
            });
    });
document.getElementById("hitButton").addEventListener("click", function (event) {

    console.log("Spieler drückt hitButton");
    connection
        .invoke("hit",slotid)
        .catch(function (err) {
            return console.error(err.toString());
        });
});
document.getElementById("endTurn").addEventListener("click", function (event) {

    console.log("Spieler beendet seinen Zug");
    connection
        .invoke("endTurn",slotid)
        .catch(function (err) {
            return console.error(err.toString());
        });
});

function disableBet() {
    document.getElementById("chipsDiv").style.display = "none";
}


function enableBet() {
    document.getElementById("chipsDiv").style.display = "flex";
}

function setBalance(_double amount) {
    if (amount > 0.0) {
        document.getElementById("money").innerHTML = "Guthaben: " + amount.toString();
    }
}