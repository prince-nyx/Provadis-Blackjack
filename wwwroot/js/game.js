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
        addCardToPlayer(args[0], args[1], args[2]);
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
        addDealerCard(args[0], args[1]);
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
        showDealerCards(args[0]);
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
connection.on("load", function (args) {
    try {
        load(args[0], args[1], args[2]);
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
connection.on("markActivePlayer", function (args) {
    try {
        markActivePlayer(args[0]);
    } catch (err) {
        console.log("ERROR(markActivePlayer) " + err.message);
    }
});

connection.on("markUserSlot", function (args) {
    try {
        markUserSlot(args[0]);
    } catch (err) {
        console.log("ERROR(markUserSlot) " + err.message);
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
    var cardSlot;
    for (let i = 1; i <= 7; i++) {
        for (let x = 1; x <= 11; x++) {
            cardSlot = document.getElementById("Spieler" + i + "-Card" + x);
            cardSlot.src = `/images/kartenruecken.png`;
            cardSlot.classList.remove("visible");
        }
    }

    for (let x = 1; x <= 11; x++) {
        cardSlot = document.getElementById("Dealer-Card" + x);
        cardSlot.src = `/images/kartenruecken.png`;
        cardSlot.classList.remove("visible");
    }
}


function addCardToPlayer(slotID, card, cardslot) {
    let cardSlot;
    slotID++;
    cardSlot = document.getElementById("Spieler" + slotID + "-Card" + cardslot);
    cardSlot.src = `/images/card/${card}.png`;
    cardSlot.classList.add("visible");
    /*
    for (let i = 1; i <= 11; i++) {
        cardSlot = document.getElementById("Spieler" + slotID + "-Card" + i);
        if (!cardSlot.classList.contains("visible")) {
            cardSlot.src = `/images/card/${card}.png`;
            cardSlot.classList.add("visible");
            break;
        }
    }
    */
}

function addDealerCard(card, cardslot) {
    let cardSlot = null;
    cardSlot = document.getElementById("Dealer-Card" + cardslot);
    if (cardslot == 1)
        cardSlot.src = `/images/kartenruecken.png`;
    else
        cardSlot.src = `/images/card/${card}.png`;
    cardSlot.classList.add("visible");
    /*for (let i = 1; i <= 11; i++) {
        cardSlot = document.getElementById("Dealer-Card" + i);
        if (!cardSlot.classList.contains("visible")) {
            if (i == 1)
                cardSlot.src = `/images/kartenruecken.png`;
            else 
                cardSlot.src = `/images/card/${card}.png`;
            cardSlot.classList.add("visible");
            break;
        }
    }
    */
}

function showDealerCards(cardname) {
    var cardSlot = document.getElementById("Dealer-Card1");
    cardSlot.src = "/images/card/"+cardname+".png";
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
    closeWinnerScreen();
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

function load(amount, name, gamecode) {
    setBalance(amount);
    setName(name);
    document.getElementById("code").innerHTML = "Gamecode: "+gamecode;
}

function showResult(headline, result) {
    document.getElementById("resultHeadline").innerHTML = headline;
    document.getElementById("resultAmount").innerHTML = result;
    document.getElementById("resultScreen").classList.add("visible");
    document.getElementById("Dealer").classList.remove("onTurn");

}

function startTurn() {
    document.getElementById("turnbuttons").classList.add("visible");

}

function endTurn() {
    document.getElementById("turnbuttons").classList.remove("visible");
}


function assignPlayerToSlot(slotid, username) {
    slotid++;
    document.getElementById("Spieler" + slotid + "-name").innerHTML = username;
    document.getElementById("Spieler" + slotid).classList.add("activeSlot");
}


function unassignPlayer(slotid) {
    slotid++;
    document.getElementById("Spieler" + slotid + "-name").innerHTML = "";
    document.getElementById("Spieler" + slotid).classList.remove("activeSlot");
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

function markUserSlot(slotid) {
    slotid++;
    var slot = document.getElementById("Spieler" + slotid +"-container");
    slot.classList.add("myPlayer");

}

function markActivePlayer(slotid) {
    for (var i = 1; i <= 7; i++) {
        var slot = document.getElementById("Spieler" + i);
        if (slot.classList.contains("onTurn")) {
            slot.classList.remove("onTurn");
            console.log("reset Spieler" + i);
        }
    }
    var dealerslot = document.getElementById("Dealer");
    if (dealerslot.classList.contains("onTurn")) {
        dealerslot.classList.remove("onTurn");
    console.log("reset Dealer");

    }

    slotid++;
    if (slotid == 8) {
        document.getElementById("Dealer").classList.add("onTurn");
    }
    else {
        console.log("set Spieler" + slotid);
        document.getElementById("Spieler" + slotid).classList.add("onTurn");
    }
}

function closeWinnerScreen() {
    document.getElementById("resultScreen").classList.remove("visible");
}
