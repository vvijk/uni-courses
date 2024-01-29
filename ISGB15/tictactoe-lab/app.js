"use strict";

/**
 * Globalt objekt som innehåller de attribut som ni skall använda.
 * Initieras genom anrop till funktionern initGlobalObject().
 */
let oGameData = {};

/**
 * Initerar det globala objektet med de attribut som ni skall använda er av.
 * Funktionen tar inte emot några värden.
 * Funktionen returnerar inte något värde.
 */
oGameData.initGlobalObject = function() {

    //Datastruktur för vilka platser som är lediga respektive har brickor
    oGameData.gameField = Array('', '', '', '', '', '', '', '', '');
    
    /* Testdata för att testa rättningslösning */
    //oGameData.gameField = Array('', '', '', 'O', 'O', 'O', '', '', '');
    //oGameData.gameField = Array('', 'O', '', '', 'O', '', '', 'O', '');
    //oGameData.gameField = Array('X', '', '', '', 'X', '', '', '', 'X');
    //oGameData.gameField = Array('', '', 'X', '', 'X', '', 'X', '', '');
    //oGameData.gameField = Array('X', 'O', 'X', '0', 'X', 'O', 'O', 'X', 'O');

    //Indikerar tecknet som skall användas för spelare ett.
    oGameData.playerOne = "X";

    //Indikerar tecknet som skall användas för spelare två.
    oGameData.playerTwo = "O";

    //Kan anta värdet X eller O och indikerar vilken spelare som för tillfället skall lägga sin "bricka".
    oGameData.currentPlayer = "";

    //Nickname för spelare ett som tilldelas från ett formulärelement,
    oGameData.nickNamePlayerOne = "";

    //Nickname för spelare två som tilldelas från ett formulärelement.
    oGameData.nickNamePlayerTwo = "";

    //Färg för spelare ett som tilldelas från ett formulärelement.
    oGameData.colorPlayerOne = "";

    //Färg för spelare två som tilldelas från ett formulärelement.
    oGameData.colorPlayerTwo = "";

    //"Flagga" som indikerar om användaren klickat för checkboken.
    oGameData.timerEnabled = false;

    //Timerid om användaren har klickat för checkboxen. 
    oGameData.timerId = null;

}

/**
 * Kontrollerar för tre i rad.
 * Returnerar 0 om det inte är någon vinnare, 
 * Olika vinstscenarier: 0, 1, 2 | 0, 4, 8| 0, 3, 6 | så om gameField[0] = 'X', kontrollera efter minst 5 spelade rundor om
 * returnerar 1 om spelaren med ett kryss (X) är vinnare,
 * returnerar 2 om spelaren med en cirkel (O) är vinnare eller
 * returnerar 3 om det är oavgjort.
 * Funktionen tar inte emot några värden.
 */
oGameData.checkHorizontal = function(){
    for(let i = 0; i < oGameData.gameField.length; i++){
        if(i % 3 == 0){
            if(oGameData.gameField[i] == oGameData.gameField[i+1] && oGameData.gameField[i+1] == oGameData.gameField[i+2] && oGameData.gameField[i+2] == oGameData.playerOne){
                return 1;
            }   
            else if(oGameData.gameField[i] == oGameData.gameField[i+1] && oGameData.gameField[i+1] == oGameData.gameField[i+2] && oGameData.gameField[i+2] == oGameData.playerTwo){
                return 2;
            }
        }
    }
}
oGameData.checkVertical = function(){
    for(let i = 0; i < oGameData.gameField.length / 3; i++){
        if(oGameData.gameField[i] == oGameData.gameField[i+3] && oGameData.gameField[i+3] == oGameData.gameField[i+6] && oGameData.gameField[i+6] == oGameData.playerOne){
            return 1;
        }
        else if(oGameData.gameField[i] == oGameData.gameField[i+3] && oGameData.gameField[i+3] == oGameData.gameField[i+6] && oGameData.gameField[i+6] == oGameData.playerTwo){
            return 2;
        }
    }
}
oGameData.checkDiagonalLeftToRight = function(){
    if(oGameData.gameField[0] == oGameData.gameField[4] && oGameData.gameField[4] == oGameData.gameField[8] && oGameData.gameField[8] == oGameData.playerOne){
        return 1;
    }
    else if(oGameData.gameField[0] == oGameData.gameField[4] && oGameData.gameField[4] == oGameData.gameField[8] && oGameData.gameField[8] == oGameData.playerTwo){
        return 2;
    }
}
oGameData.checkDiagonalRightToLeft = function(){
    if(oGameData.gameField[2] == oGameData.gameField[4] && oGameData.gameField[4] == oGameData.gameField[6] && oGameData.gameField[6] == oGameData.playerOne){
        return 1;
    }
    else if(oGameData.gameField[2] == oGameData.gameField[4] && oGameData.gameField[4] == oGameData.gameField[6] && oGameData.gameField[6] == oGameData.playerTwo){
        return 2;
    }
}
oGameData.checkForDraw = function(){
 for(let i = 0; i < oGameData.gameField.length; i++){
     if(oGameData.gameField[i] == ''){
        return 0; 
     }
 }
 return 3;
}
oGameData.checkForGameOver = function() {
    
    if (oGameData.checkHorizontal()){
        return oGameData.checkHorizontal();
    }
    if (oGameData.checkVertical()){
        return oGameData.checkVertical();
    }
    if (oGameData.checkDiagonalLeftToRight()){
        return oGameData.checkDiagonalLeftToRight();
    }
    if (oGameData.checkDiagonalRightToLeft()){
        return oGameData.checkDiagonalRightToLeft();
    }
    if (oGameData.checkForDraw()) {
        return oGameData.checkForDraw();
    }
    return 0;
}

window.addEventListener('load', function(){
    oGameData.initGlobalObject();
    timerBody();
    document.querySelector('#gameArea').classList.add('class', 'd-none');
    document.querySelector('#newGame').addEventListener('click', function(){
        if(validateForm()){
            if(oGameData.timerEnabled){
                startTimer();
                console.log("Timer started");
            }
            initiateGame();
        }
    })
});

function validateForm(){
    let min = 5; // Minsta längd på namn
    let white = "#ffffff"; //Förbjuden färg
    let black = "#000000"; //Förbjuden färg
    try {
        // NAMNCHECK
        if(document.querySelector('#nick1').value.length < min){
            throw("NickName1 är för kort");
        }
        if(document.querySelector('#nick2').value.length < min){
            throw("Nickname2 är för kort");
        }
        if(document.querySelector('#nick2').value === document.querySelector('#nick1').value){
            throw("Samma namn på nickname");
        }
        // FÄRGCHECK
        let colorP1 = document.querySelector('#color1').value;
        let colorP2 = document.querySelector('#color2').value;
        if(colorP1 == white || colorP1 == black){
            throw("Spelare 1 har en förbjuden färg");
        }
        if(colorP2 == white || colorP2 == black){
            throw("Spelare 2 har en förbjuden färg");
        }
        if(colorP1 === colorP2){
            throw("Spelare får inte ha samma färg!!!!!½!!!!");
        }
        return true;
    } catch (e){
        console.log(e);
        document.querySelector('#errorMsg').textContent = e;
        return false;
    }
}
function initiateGame(){
    document.querySelector('form').classList.add('class', 'd-none');
    document.querySelector('#gameArea').classList.remove('class', 'd-none');
    document.querySelector('#errorMsg').classList.add('class', 'd-none');
    oGameData.nickNamePlayerOne = document.querySelector('#nick1').value;
    oGameData.nickNamePlayerTwo = document.querySelector('#nick2').value;
    oGameData.colorPlayerOne = document.querySelector('#color1').value;
    oGameData.colorPlayerTwo = document.querySelector('#color2').value;

    let boxar = document.querySelectorAll('td');
    boxar.forEach(function(box){
        box.textContent='';
        box.setAttribute('style', 'background-color: white');
    });
    let playerChar;
    let playerName;
    if(Math.random() < 0.5){
        playerChar = oGameData.playerOne;
        playerName = oGameData.nickNamePlayerOne;
        oGameData.currentPlayer = oGameData.playerOne;
    } else {
        playerChar = oGameData.playerTwo;
        playerName = oGameData.nickNamePlayerTwo;
        oGameData.currentPlayer = oGameData.playerTwo;
    }
    document.querySelector('.jumbotron >h1').textContent = "Aktuell spelare är " + playerName;
    document.querySelector('table').addEventListener('click', executeMove);
}
function executeMove(oEvent){
    let cells = document.querySelectorAll('td');
    for(let i = 0; i < cells.length; i++){
        if(oEvent.target.getAttribute('data-id') == i){     //Hämta data-id
            if(oGameData.gameField[i] == ''){   //Om platsen är '' (tom)
                cells[i].textContent = oGameData.currentPlayer;     //CurrentPlayer-markör tar platsen
                oGameData.gameField[i] = oGameData.currentPlayer;   //Även i gameField

                if(oGameData.currentPlayer == oGameData.playerOne){
                    cells[i].style = 'background-color: ' + oGameData.colorPlayerOne;
                    oGameData.currentPlayer = oGameData.playerTwo;
                    document.querySelector('.jumbotron >h1').textContent = "Aktuell spelare är " + oGameData.nickNamePlayerTwo;
                } else if(oGameData.currentPlayer == oGameData.playerTwo){
                    cells[i].style = 'background-color: ' + oGameData.colorPlayerTwo;
                    oGameData.currentPlayer = oGameData.playerOne;
                    document.querySelector('.jumbotron >h1').textContent = "Aktuell spelare är " + oGameData.nickNamePlayerOne;
                }
                if(oGameData.timerEnabled){ //om timern är på så blir detta som en timer-reset.
                    clearTimeout(oGameData.TimerId); //Avslutar timern
                    startTimer(); //Startar timern igen
                }
                if(oGameData.checkForGameOver()){   //Kollar om spelet är slut
                    if(oGameData.timerEnabled){  
                        document.getElementById('timerCheckBox').checked = false;
                        clearTimeout(oGameData.TimerId); //Avslutar timern.

                    }
                    document.querySelector('table').removeEventListener('click', executeMove);
                    document.querySelector('form').classList.remove('class', 'd-none');
                    //Kollar vem som vunnit
                    if(oGameData.checkForGameOver() == 2){
                        document.querySelector('.jumbotron >h1').textContent = oGameData.nickNamePlayerTwo + '(' + oGameData.playerTwo + ')' + ' vinner spelet! Spela igen?';
                    }else if (oGameData.checkForGameOver() == 1) {
                        document.querySelector('.jumbotron >h1').textContent = oGameData.nickNamePlayerOne + '(' + oGameData.playerOne + ')' + ' vinner spelet! Spela igen?';
                    } else if(oGameData.checkForGameOver() == 3){
                        document.querySelector('.jumbotron >h1').textContent = 'Oavgjort! Spela igen?';
                    }
                    document.querySelector('#gameArea').classList.add('class', 'd-none');
                    oGameData.initGlobalObject();
                }
            }
        }
    }
}
function timerBody(){

    let timerdiv = document.createElement("div")
    timerdiv.classList.add('class', 'col-6');

    let timerCheckBox = document.createElement("input");
    timerCheckBox.setAttribute("type","checkbox");
    timerCheckBox.setAttribute("id", "timerCheckBox");
    timerCheckBox.setAttribute("name", "timerCheckBox");
    timerCheckBox.classList.add('class','form-check-input');

    timerCheckBox.addEventListener('change', function() {
        if(this.checked){
            oGameData.timerEnabled = true;
        } else {
            oGameData.timerEnabled = false;
        }
    });

    let checkBoxLabel = document.createElement("label")
    checkBoxLabel.setAttribute("for","timerCheckBox");
    checkBoxLabel.classList.add('class','form-check-label');

    let text = document.createTextNode("Spela med 5 sekunders betänketid?");
    checkBoxLabel.appendChild(text);

    let parentDiv = document.querySelector("#divInForm");
    let btnDiv = document.querySelector('#divWithA');

    timerdiv.appendChild(timerCheckBox);
    timerdiv.appendChild(checkBoxLabel);
    parentDiv.insertBefore(timerdiv, btnDiv);
}
function startTimer(){ //Startar timeout som byter spelare efter 5s
    oGameData.TimerId = setTimeout(switchPlayerTurn, 5000);
}
function switchPlayerTurn(){ //byter spelare och stannar timern och startar om den.
    if(oGameData.currentPlayer == oGameData.playerOne){
        oGameData.currentPlayer = oGameData.playerTwo;
        document.querySelector('.jumbotron >h1').textContent = "Aktuell spelare är " + oGameData.nickNamePlayerTwo;
    } else if(oGameData.currentPlayer == oGameData.playerTwo){
        oGameData.currentPlayer = oGameData.playerOne;
        document.querySelector('.jumbotron >h1').textContent = "Aktuell spelare är " + oGameData.nickNamePlayerOne;
    }
    clearTimeout(oGameData.TimerId);
    startTimer();
}