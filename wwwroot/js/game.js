console.log("Verbindung wird aufgebaut");
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
connection.on("gamestarting", function (args) {
    try {
        resetCards();
        document.getElementById("startbuttons").classList.remove("visible");
    } catch (err) {
        console.log("ERROR(gamestarting) " + err.message);
    }
});
connection.on("addCardToPlayer", function (args) {
    try {
        addCardToPlayer(args[0], args[1]);
    } catch (err) {
        console.log("ERROR(addCardToPlayer) " + err.message);
    }
});
connection.on("setCardSum", function (args) {
    try {
        setCardSum(args[0], args[1]);
    } catch (err) {
        console.log("ERROR(setCardSum) " + err.message);
    }
});
connection.on("assignPlayer", function (args) {
    try {
        assignPlayerToSlot(args[0], args[1]);
    } catch (err) {
        console.log("ERROR(assignPlayer) " + err.message);
    }
});
connection.on("unassignPlayer", function (args) {
    try {
        unassignPlayer(args[0]);
    } catch (err) {
        console.log("ERROR(unassignPlayer) " + err.message);
    }
});
connection.on("setBet", function (args) {
    try {
        setBet(args[0], args[1]);
    } catch (err) {
        console.log("ERROR(setBet) " + err.message);
    }
});
connection.on("addDealerCard", function (args) {
    try {
        addDealerCard(args[0]);
    } catch (err) {
        console.log("ERROR(addDealerCard) " + err.message);
    }
});
connection.on("enableBet", function (args) {
    try {
        enableBet();
    } catch (err) {
        console.log("ERROR(enableBet) " + err.message);
    }
});
connection.on("disableBet", function (args) {

    try {
        disableBet();
    } catch (err) {
        console.log("ERROR(disableBet) " + err.message);
    }
});
connection.on("showDealerCards", function (args) {
    try {
        showDealerCards();
    } catch (err) {
        console.log("ERROR(showDealerCards) "+err.message);
    }
});
connection.on("endTurn", function (args) {
    try {
        endTurn();
    } catch (err) {
        console.log("ERROR(endTurn) " + err.message);
    }
});
connection.on("startTurn", function (args) {
    try {
        startTurn(args[0]);
    } catch (err) {
        console.log("ERROR(startTurn) " + err.message);
    }
});
connection.on("setbBalance", function (args) {
    try {
        setbBalance(args[0]);
    } catch (err) {
        console.log("ERROR(setbBalance) " + err.message);
    }
});
connection.on("showResult", function (args) {
    try {
        showResult(args[0], args[1]);
    } catch (err) {
        console.log("ERROR(showResult) " + err.message);
    }
});
connection.on("showStartButton", function (args) {
    try {
        showStartButton();
    } catch (err) {
        console.log("ERROR(showStartButton) " + err.message);
    }
});
connection.on("load", function (amount, username, gamecode) {
    try {
        load(amount, username, gamecode);
    } catch (err) {
        console.log("ERROR(load) " + err.message);
    }
});
connection.on("console", function (message) {
    try {
        console.log(message);
    } catch (err) {
        console.log("ERROR(console) " + err.message);
    }
});
//STOP BACKEND EVENTS


function showStartButton() {
    document.getElementById("startbuttons").classList.add("visible");
}

function disableStartButton() {
    document.getElementById("startbuttons").classList.remove("visible");

}

function resetCards() {
    for (let i = 1; i <= 7; i++) {
        for (let x = 1; x <= 11; x++) {
            cardSlot = document.getElementById("Spieler" + i + "-Card" + x);
            cardSlot.src = `/images/kartenruecken.png`;
        }
    }

    for (let x = 1; x <= 11; x++) {
        cardSlot = document.getElementById("Dealer-Card" + x);
        cardSlot.src = `/images/kartenruecken.png`;
    }
}


function addCardToPlayer(slotID, card) {
    let slot = null;
    let cardSlot = null;
    var player = `Spieler${slotID + 1}`;
    switch (slotID) {
        case 8:
            slot = document.getElementById("Dealer");
            break;
        default:
            slot = document.getElementById(player);
            break;
    }

    for (let i = 1; i <= 11; i++) {
        cardSlot = document.getElementById(player + "-Card" + i);
        if (cardSlot.src == "/images/kartenruecken.png") {
            cardSlot.src = `/images/card/${card}.png`;
            break;
        }
    }
}

function addDealerCard(card, isHidden) {
    let cardSlot = null;
    for (let i = 1; i <= 11; i++) {
        cardSlot = document.getElementById("Dealer" + i + "-Card" + i);
        if (isHidden == "False" && i > 1) {
            if (cardSlot.src == "/images/kartenruecken.png") {
                cardSlot.src = `/images/card/${card}.png`;
                break;
            } 
        } else {
            cardSlot.src = `/images/kartenruecken.png`;
            cardSlot.alt = card;
            break;
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

document.getElementById("standButton").addEventListener("click", function (event) {
    connection
        .invoke("stand", getCookie("userid"))
        .catch(function (err) {
            return console.error(err.toString());
        });

});


function disableBet() {
    document.getElementById("chipsDiv").classList.remove("visible");
}


function enableBet() {
    document.getElementById("chipsDiv").classList.add("visible");
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
}

function showResult(headline, result) {
    document.getElementById("resultHeadline").innerHTML = headline;
    document.getElementById("resultAmount").innerHTML = result;
    document.getElementById("resultScreen").style.visibility = "visible";
}

function startTurn() {
    document.getElementById("turnButtons").classList.add("visible");

}

function endTurn() {
    document.getElementById("turnButtons").classList.remove("visible");
}


function assignPlayerToSlot(slotid, username) {
    document.getElementById("spieler"+slotid+"-name").innerHTML = username;
}


function unassignPlayer(slotid) {
    document.getElementById("spieler" + slotid + "-name").innerHTML = "";
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

/*function setCardSum(slotid, amount) {
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


    // Target the dealer slot
    const dealerSumElement = document.getElementById('sumDealer');
    if (dealerSumElement) {
        let dealerSum = parseInt(dealerSumElement.textContent.trim().split(':')[1]);
        if (isNaN(dealerSum)) {
            dealerSum = 0;
        }
        dealerSum += amount;
        dealerSumElement.textContent = `Kartensumme: ${dealerSum}`;

        const dealerAddedAmountElement = document.getElementById('addedAmountDealer');
        dealerAddedAmountElement.textContent = `Höhe der gezogenen Karte: +${amount}`;
    }

    // Target the benutzer slot
    const benutzerSumElement = document.getElementById('sumBenutzer');
    if (benutzerSumElement) {
        let benutzerSum = parseInt(benutzerSumElement.textContent.trim().split(':')[1]);
        if (isNaN(benutzerSum)) {
            benutzerSum = 0;
        }
        benutzerSum += amount;
        benutzerSumElement.textContent = `Kartensumme: ${benutzerSum}`;

        const benutzerAddedAmountElement = document.getElementById('addedAmountBenutzer');
        benutzerAddedAmountElement.textContent = `Höhe der gezogenen Karte: +${amount}`;
    }
}









