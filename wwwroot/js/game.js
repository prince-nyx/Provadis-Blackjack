console.log("Verbindung wird aufgebaut ...");
var connection = new signalR.HubConnectionBuilder().withUrl("/GameHub").build();

//Die Verbindung wurde aufgebaut und ruft nun diese Funktion auf:
connection.start().then(function () {
    console.log("Verbindung erfolgreich.")
    connection
        .invoke("onConnection", getCookie("userid"))
        .catch(function (err) {
            return console.error(err.toString());
        });
    setInterval(updateTask, 500);
}).catch(function (err) {
    console.log("Verbindung fehlgeschlagen");

    //falls die Verbindung fehlschlägt. man könnte z.b. die seite neu laden lassen.
    return console.error(err.toString());

});
connection.on("console", function (message) {
    console.log(message);
});


function updateTask() {
    connection
        .invoke("update", getCookie("userid"))
        .catch(function (err) {
            return console.error(err.toString());
        });
}

function addCardToPlayer(slotID, card) {
    let slot = null;
    let cardSlot = null;

    switch (slotID) {
        case 4:
            slot = document.getElementById("Benutzer");
            break;
        default:
            slot = document.getElementById(`Spieler${slotID + 1}`);
            break;
    }

    for (let i = 1; i <= 11; i++) {
        cardSlot = slot.getElementsByClassName(`OfClubs${i}`)[0];
        if (cardSlot.src == "") {
            cardSlot.src = `/images/card/${card}.png`;
            break;
        }
    }
}

function addDealerCard(card, isHidden) {
    let slot = document.getElementById("Dealer");
    let cardSlot = null;

    for (let i = 1; i <= 11; i++) {
        cardSlot = slot.getElementsByClassName(`OfClubs${i}`)[0];
        if (cardSlot.src == "") {
            if (!isHidden) {
                cardSlot.src = `/images/card/${card}.png`;
                break;
            }
            else {
                cardSlot.src = `/images/design rueckseite.png`;
                break;
            }
        }
    }
}

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

connection.on("SetCardImage", function (args) {

    Console.log("test");
    var image = document.getElementById("Spieler1_Card_"+args[0]);
    image.src = "/images/card/" + args[1] +".png";

});


connection.on("SetCurrentPlayer", function (args) {

    Console.log(args[0]);

});


document.getElementById("startButton").addEventListener("click", function (event) {

    console.log("Game startet ..." + getCookie("userid"));
        connection
            .invoke("startGame", getCookie("userid"))
            .catch(function (err) {
                return console.error(err.toString());
            });
});


document.getElementById("hitButton").addEventListener("click", function (event) {

    console.log("Spieler " + getCookie("userid") +" drückt hitButton");
    connection
        .invoke("hit", getCookie("userid"))
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

document.getElementById("standButton").addEventListener("click", function (event) {

    console.log("Spieler zieht keine Karte");
    connection
        .invoke("stand", slotid)
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

function setBalance(amount) {    
    document.getElementById("money").innerHTML = "Guthaben: " + amount.toString() + "€";
}

function setName(name) {
    document.getElementById("username").innerHTML = "Viel Erfolg " + name;
}

function load(amount, name) {
    setBalance(amount);
    setName(name);
    disableBet();
}

function showResult(headline, result) {
    document.getElementById("resultHeadline").innerHTML = headline;
    document.getElementById("resultAmount").innerHTML = result;
    document.getElementById("resultScreen").style.visibility = "visible";
}

function startTurn() {
    enableBet();
}

function endTurn() {
    disableBet();
}