﻿console.log("Verbindung wird aufgebaut");
var connection = new signalR.HubConnectionBuilder().withUrl("/GameHub").build();

//Die Verbindung wurde aufgebaut und ruft nun diese Funktion auf:
connection.start().then(function () {
    console.log("Verbindung erfolgreich.")
    connection
        .invoke("onConnection", getCookie("userid"))
        .catch(function (err) {
            return console.error(err.toString());
        });
}).catch(function (err) {
    console.log("Verbindung fehlgeschlagen");

    //falls die Verbindung fehlschlägt. man könnte z.b. die seite neu laden lassen.
    return console.error(err.toString());

});

function updateTask() {
    connection
        .invoke("update", getCookie("userid"))
        .catch(function (err) {
            return console.error(err.toString());
        });
}
setInterval(updateTask, 500);


//START BACKEND EVENTS
connection.on("addCardToPlayer", function (args) {
    try {
        addCardToPlayer(args[0], args[1]);
    } catch (err) {
        console.log(err.message);
    }
});
connection.on("setCardSum", function (args) {
    try {
        setCardSum(args[0], args[1]);
    } catch (err) {
        console.log(err.message);
    }
});
connection.on("assignPlayer", function (args) {
    try {
        assignPlayer(args[0], args[1]);
    } catch (err) {
        console.log(err.message);
    }
});
connection.on("unassignPlayer", function (args) {
    try {
        unassignPlayer(args[0]);
    } catch (err) {
        console.log(err.message);
    }
});
connection.on("setBet", function (args) {
    try {
        setBet(args[0], args[1]);
    } catch (err) {
        console.log(err.message);
    }
});
connection.on("addDealerCard", function (args) {
    try {
        addDealerCard(args[0]);
    } catch (err) {
        console.log(err.message);
    }
});
connection.on("enableBet", function (args) {
    try {
        enableBet();
    } catch (err) {
        console.log(err.message);
    }
});
connection.on("disableBet", function (args) {
    try {
        disableBet();
    } catch (err) {
        console.log(err.message);
    }
});
connection.on("showDealerCards", function (args) {
    try {
        showDealerCards();
    } catch (err) {
        console.log(err.message);
    }
});
connection.on("endTurn", function (args) {
    try {
        endTurn();
    } catch (err) {
        console.log(err.message);
    }
});
connection.on("startTurn", function (args) {
    try {
        startTurn(args[0]);
    } catch (err) {
        console.log(err.message);
    }
});
connection.on("setbBalance", function (args) {
    try {
        setbBalance(args[0]);
    } catch (err) {
        console.log(err.message);
    }
});
connection.on("showResult", function (args) {
    try {
        showResult(args[0], args[1]);
    } catch (err) {
        console.log(err.message);
    }
});
connection.on("showStartButton", function (args) {
    try {
        showStartButton();
    } catch (err) {
        console.log(err.message);
    }
});
connection.on("load", function (amount, username, gamecode) {
    try {
        showStartButton(amount, username);
    } catch (err) {
        console.log(err.message);
    }
});
connection.on("console", function (message) {
    try {
        console.log(message);
    } catch (err) {
        console.log(err.message);
    }
});
//STOP BACKEND EVENTS


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
                cardSlot.alt = card;
                break;
            }
        }
    }
}

function showDealerCards() {
    let hiddenCard = document.getElementById("Dealer").getElementsByClassName(`OfClubs1`)[0];
    hiddenCard.src = `/images/card/${hiddenCard.alt}.png`;
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
        .invoke("endTurn", getCookie("userid"))
        .catch(function (err) {
            return console.error(err.toString());
        });
});

document.getElementById("standButton").addEventListener("click", function (event) {
    connection
        .invoke("stand", getCookie("userid"))
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
    document.getElementById("money").innerHTML = "Guthaben: " + amount + "€";
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


function assignPlayerToSlot(slotid, username) {
    switch (slotid) {
        case 0:
            document.getElementById("spieler1-name").innerHTML = username;
            break;
        case 1:
            document.getElementById("spieler2-name").innerHTML = username;
            break;
        case 2:
            document.getElementById("spieler3-name").innerHTML = username;
            break;
        case 3:
            document.getElementById("spieler4-name").innerHTML = username;
            break;
        case 4:
            document.getElementById("spieler5-name").innerHTML = username;
            break;
        case 5:
            document.getElementById("spieler6-name").innerHTML = username;
            break;
        case 6:
            document.getElementById("spieler7-name").innerHTML = username;
    }
}


function unassignPlayer(slotid) {
    switch (slotid) {
        case 0:
            document.getElementById("spieler1-name").innerHTML = "Spieler 1";
            break;
        case 1:
            document.getElementById("spieler2-name").innerHTML = "Spieler 2";
            break;
        case 2:
            document.getElementById("spieler3-name").innerHTML = "Spieler 3";
            break;
        case 3:
            document.getElementById("spieler4-name").innerHTML = "Spieler 4";
            break;
        case 4:
            document.getElementById("spieler5-name").innerHTML = "Spieler 5";
            break;
        case 5:
            document.getElementById("spieler6-name").innerHTML = "Spieler 6";
            break;
        case 6:
            document.getElementById("spieler7-name").innerHTML = "Spieler 7";
    }
}

//Einsatz bei drücken der Chips hochzählen und nur die nutzbaren Chip anzeigen lassen.
let playerCurrency = 12;
let totalBet = 0;
const totalAmountElement = document.getElementById('totalAmount');
const chipImages = document.querySelectorAll('.pokerchips img');

function hideChipImages() {
    chipImages.forEach(chipImage => {
        const chipValue = parseInt(chipImage.getAttribute('onclick').match(/\d+/)[0]);
        if (playerCurrency < chipValue || playerCurrency < totalBet + chipValue) {
            chipImage.style.display = 'none';
            chipImage.removeAttribute('onclick');
        }
    });
}

hideChipImages();


function setBet(amount) {
    if (playerCurrency >= totalBet + amount) {
        totalBet += amount;
        totalAmountElement.textContent = totalBet;
        hideChipImages();
        return totalBet
    }
}





//setCardSum testen
/*setCardSum(1, 10);
const sumElement = document.getElementById('sumPlayer1');
console.log(sumElement.textContent);
*/

function setCardSum(slotid, amount) {
    const sumElement = document.getElementById(`sumPlayer${slotid}`);
    if (sumElement) {
        let currentSum = parseInt(sumElement.textContent.trim().split(':')[1]);
        if (isNaN(currentSum)) {
            currentSum = 0;
        }
        currentSum += amount;
        sumElement.textContent = `Kartensumme: ${currentSum}`;

        const addedAmountElement = document.getElementById(`addedAmountPlayer${slotid}`);
        addedAmountElement.textContent = `Höhe der gezogenen Karte: +${amount}`;
    }
}











