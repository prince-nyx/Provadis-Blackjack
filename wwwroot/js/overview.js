﻿console.log("Verbindung wird aufgebaut ...");
var connection = new signalR.HubConnectionBuilder().withUrl("/GameHub").build();

//Die Verbindung wurde aufgebaut und ruft nun diese Funktion auf:
connection.start().then(function () {
    console.log("Verbindung erfolgreich.")
    connection
        .invoke("onConnectionToOverview", getCookie("userid"))
        .catch(function (err) {
            return console.error(err.toString());
        });
}).catch(function (err) {
    console.log("Verbindung fehlgeschlagen");

    //falls die Verbindung fehlschlägt. man könnte z.b. die seite neu laden lassen.
    return console.error(err.toString());
});

connection.on("load", function (amount, name) {
    try {
        load(amount, name);
    } catch (err) {
        console.log("ERROR(load) " + err.message);
    }
});

document.getElementById("logout").addEventListener("click", function (event) {

    console.log("Spieler " + getCookie("userid") + " drückt logout button");
    connection
        .invoke("logout", getCookie("userid"))
        .catch(function (err) {
            return console.error(err.toString());
        });
    window.location.replace("index");
});


connection.on("console", function (message) {
    try {
        console.log(message);
    } catch (err) {
        console.log("ERROR(console) " + err.message);
    }
});

function getCookie(cname) {
    let name = cname + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let ca = decodedCookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function load(amount, name) {
    console.log("JS"+name);
    setBalance(amount);
    setName(name);
}

function setBalance(amount) {
    document.getElementById("money").innerHTML = amount + "€";
}

function setName(name) {
    document.getElementById("username").innerHTML = name;
}


$(document).ready(function () {
    $('.code').keyup(function (event) {
        // Convert the value to uppercase and update the input
        $(this).val($(this).val().toUpperCase());

        // Check if the input's value length has reached the maximum length
        if ($(this).val().length >= $(this).attr('maxlength')) {
            // Find the next input element and give it focus
            $(this).next('.code').focus();
        }

        // Check if the Backspace key was pressed
        if (event.keyCode === 8) {
            // Find the previous input element and give it focus
            $(this).prev('.code').focus();
        }
    });
});