# Summary of Tic-Tac-Toe Labs 1-4:

In the Tic-Tac-Toe labs (Steps 1-4), I've have progressively built the game, enhancing its functionality and interactivity using unobtrusive JavaScript and event listeners. Here's an overview of each step:

## Step 1: Game Logic (Lab 1)
Implemented the checkForGameOver function in app.js.
Checked for a winner by examining rows, columns, and diagonals.
Returned values indicating the game state: 0 for no winner, 1 for Player 1 (X) win, 2 for Player 2 (O) win, and 3 for a draw.

## Step 2: Validation and Initialization (Lab 2)
Validated input data (nicknames and chosen colors) using validateForm.
Utilized unobtrusive JavaScript and addEventListener() for event handling.
Initiated the game upon button click, hiding the form and revealing the game area.
Stored player names and colors in the oGameData object.
Randomized the starting player and displayed their name.
Cleared the game board and set up event listeners for moves.

## Step 3: Game Engine (Lab 3)
Developed the game engine in app.js.
Added a click listener on the game board to execute moves.
Checked for valid moves and updated the game state accordingly.
Dynamically changed the current player, updating the UI.
Checked for game completion, displaying results and offering a replay option.
In these steps, I've established the core logic, validated user inputs, and created an interactive game environment.

## Step 4: Timer Integration (Lab 4)
Integrated a timer feature during game initiation for selecting nicknames and colors.
Added a checkbox to enable time-limited moves.
If checked, allowed 5 seconds for each move; otherwise, players could take their time.
If a player did not make a move within the time limit, the turn automatically passed to the opponent.
Ensured the timer event concluded on game victory.

> [!NOTE]
> Skeleton code was provided to us as a starting point, but the main logic and implementations were developed by me.