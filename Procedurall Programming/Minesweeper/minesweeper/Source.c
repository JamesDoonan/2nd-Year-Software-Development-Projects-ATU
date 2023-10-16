#include<stdio.h>
#include <stdbool.h>
#define MAXSIDE 8
#define MAXMINES 10

int SIDE; // length of the sides of the board
int MINES; // Number of mines
int finalScore; // players final score displayed at game end
FILE* saveMyBoard;
FILE* saveRealBoard;

// initialise method used to reset the game by blank out all mines and to reset random number generator so the same config doesnt happen
void initialise(char realBoard[][MAXSIDE], char myBoard[][MAXSIDE])
{
    srand(time(NULL));

    // assign all the cells on the board to be mine-free
    for (int i = 0; i < SIDE; i++)
    {
        for (int j = 0; j < SIDE; j++)
        {
            myBoard[i][j] = realBoard[i][j] = '-';
        }
    }

    return;
}

// setupRandomMines method is used to setup the location of the 10 mines randomly on the board
void setupRandomMines(int mines[][2], char realBoard[][MAXSIDE])
{
    bool mark[MAXSIDE * MAXSIDE];

    //Memset() is a function.It copies a single character for a specified number of times to an object
    memset(mark, false, sizeof(mark));

    // For loop used to setup random mines until all random mines have been created.
    for (int i = 0; i < MINES; )
    {
        int random = rand() % (SIDE * SIDE);
        int x = random / SIDE;
        int y = random % SIDE;

        // this adds the mine at the random location set above,  if there
        // is no mine placed at this position on the board
        if (mark[random] == false)
        {
            // set the row & column indices for the Mine
            mines[i][0] = x;
            mines[i][1] = y;

            // Place the mine, mark that there is a mine at this 
            // location and increment
            realBoard[mines[i][0]][mines[i][1]] = '*';
            mark[random] = true;
            i++;
        }
    }

    return;
}

// printMinesweeperBoard method prints the current status of the board for the player
void printMinesweeperBoard(char myBoard[][MAXSIDE])
{
    int i, j;

    printf("    ");

    for (i = 0; i < SIDE; i++)
        printf("%d ", i);

    printf("\n\n");

    for (i = 0; i < SIDE; i++)
    {
        printf("%d   ", i);

        for (j = 0; j < SIDE; j++)
            printf("%c ", myBoard[i][j]);
            printf("\n");
    }
    return;
}

// inputPlayersMove method allows the player to input a move (*x is the value at x address & *y is the value at the y address)
void inputPlayersMove(int* x, int* y)
{
    // player inputs the next move they want in to the console
    printf("Where would you like to move? Enter the row value followed by a space and then the column value (row column)-> ");
    scanf("%d %d", x, y);
    
    return;
}

// isPlayerMoveValid checks whether the inputted row & col is within range and valid
// returns true if both the inputted row and column is in range
bool isPlayerMoveValid(int row, int col)
{
    return (row >= 0) && (row < SIDE) && (col >= 0) && (col < SIDE);
}

// isAMine checks whether the selected row and column has a mine or doesn't 
bool isAMine(int row, int col, char board[][MAXSIDE])
{
    if (board[row][col] == '*')
        return (true);   
    else
        return (false);
}

// playMinesweeper method is used to play the minesweeper games functionality
bool playMinesweeper(char myBoard[][MAXSIDE], char realBoard[][MAXSIDE],
    int mines[][2], int row, int col, int* movesLeft)
{
    int i, j;

    if (myBoard[row][col] != '-')
        return (false);

    // check that checks if you have opened a mine. If this is the case you lose
    if (realBoard[row][col] == '*')
    {
        myBoard[row][col] = '*';

        for (i = 0; i < MINES; i++)
            myBoard[mines[i][0]][mines[i][1]] = '*';

        // print the location of the mines on the board
        printMinesweeperBoard(myBoard);

        return (true);
    }
    else
    {
        // if a mine is not hit, calculate the number surrounding mines and print the board with these values
        int count = countSurroundingMines(row, col, mines, realBoard);
        (*movesLeft)--;

        myBoard[row][col] = count + '0';

        if (!count)
        {
            // ABOVE
            if (isPlayerMoveValid(row - 1, col) == true)
            {
                if (isAMine(row - 1, col, realBoard) == false)
                    playMinesweeper(myBoard, realBoard, mines, row - 1, col, movesLeft);
            }
            // BELOW
            if (isPlayerMoveValid(row + 1, col) == true)
            {
                if (isAMine(row + 1, col, realBoard) == false)
                    playMinesweeper(myBoard, realBoard, mines, row + 1, col, movesLeft);
            }
            // LEFT
            if (isPlayerMoveValid(row, col + 1) == true)
            {
                if (isAMine(row, col + 1, realBoard) == false)
                    playMinesweeper(myBoard, realBoard, mines, row, col + 1, movesLeft);
            }
            // RIGHT
            if (isPlayerMoveValid(row, col - 1) == true)
            {
                if (isAMine(row, col - 1, realBoard) == false)
                    playMinesweeper(myBoard, realBoard, mines, row, col - 1, movesLeft);
            }

            // Diagonal ABOVE/RIGHT
            if (isPlayerMoveValid(row - 1, col + 1) == true)
            {
                if (isAMine(row - 1, col + 1, realBoard) == false)
                    playMinesweeper(myBoard, realBoard, mines, row - 1, col + 1, movesLeft);
            }
            // Diagonal ABOVE/LEFT
            if (isPlayerMoveValid(row - 1, col - 1) == true)
            {
                if (isAMine(row - 1, col - 1, realBoard) == false)
                    playMinesweeper(myBoard, realBoard, mines, row - 1, col - 1, movesLeft);
            }
            // Diagonal BELOW/RIGHT
            if (isPlayerMoveValid(row + 1, col + 1) == true)
            {
                if (isAMine(row + 1, col + 1, realBoard) == false)
                    playMinesweeper(myBoard, realBoard, mines, row + 1, col + 1, movesLeft);
            }
            // Diagonal BELOW/LEFT
            if (isPlayerMoveValid(row + 1, col - 1) == true)
            {
                if (isAMine(row + 1, col - 1, realBoard) == false)
                    playMinesweeper(myBoard, realBoard, mines, row + 1, col - 1, movesLeft);
            }
        }

        return (false);
    }
}

// countSurroundingMines counts the number of mines in the row and columns surrounding the location inputted by the player
int countSurroundingMines(int row, int col, int mines[][2], char realBoard[][MAXSIDE])
{
    int i;
    int count = 0;
   

    // Check all the following directions only if the row and column inputted by the player is valid:

    // ABOVE
    if (isPlayerMoveValid(row - 1, col) == true)
    {
        if (isAMine(row - 1, col, realBoard) == true)
            count++;
    }
    // BELOW
    if (isPlayerMoveValid(row + 1, col) == true)
    {
        if (isAMine(row + 1, col, realBoard) == true)
            count++;
    }
    // LEFT
    if (isPlayerMoveValid(row, col + 1) == true)
    {
        if (isAMine(row, col + 1, realBoard) == true)
            count++;
    }
    // RIGHT
    if (isPlayerMoveValid(row, col - 1) == true)
    {
        if (isAMine(row, col - 1, realBoard) == true)
            count++;
    }

    // Diagonal ABOVE/RIGHT
    if (isPlayerMoveValid(row - 1, col + 1) == true)
    {
        if (isAMine(row - 1, col + 1, realBoard) == true)
            count++;
    }
    // Diagonal ABOVE/LEFT
    if (isPlayerMoveValid(row - 1, col - 1) == true)
    {
        if (isAMine(row - 1, col - 1, realBoard) == true)
            count++;
    }
    // Diagonal BELOW/RIGHT
    if (isPlayerMoveValid(row + 1, col + 1) == true)
    {
        if (isAMine(row + 1, col + 1, realBoard) == true)
            count++;
    }
    // Diagonal BELOW/LEFT
    if (isPlayerMoveValid(row + 1, col - 1) == true)
    {
        if (isAMine(row + 1, col - 1, realBoard) == true)
            count++;
    }

    finalScore += count;
    
    return (count);
}

// method that loads from a saved game and creates a new mindsweeper board game from the loaded data
void loadFromSavedGame()
{
    printf("\n Loading previously saved game... ");

    // Still needs to be done
    // Read the contents saved from saveMyBoard.txt (This contains the inputted guesses by the player, row & columns)
    // Read the contents saved from saveRealBoard.txt (This contains the locations of the mines)
    saveMyBoard = fopen("saveMyBoard.txt", "r");
    saveRealBoard = fopen("saveRealBoard.txt", "r");

    if (saveMyBoard == NULL || saveRealBoard == NULL)
    {
        printf("Sorry there was an issue loading data files and they could not be opened.\n");
    }
    else
    {
        // Create new Board -
#
        // Set size of Board 
        SIDE = 8;

        // Set the number of desired mines
        MINES = 10;

        // Create new Real Board and My Board arrays 
        char realBoard[MAXSIDE][MAXSIDE], myBoard[MAXSIDE][MAXSIDE];

        // Load the data from scanMyBoard to the arrays above - something like ...

        //while (!feof(saveMyBoard))
        //{
        //    int numInputs = fscanf(saveMyBoard, "%c", myBoard);
        //    printf("Mindsweeper Board : \n");
        //    printf(myBoard);
        //}
        //fclose(saveMyBoard);

        // Load the data from scanRealBoard to the arrays above - contains mine locations ... something like ...

        //while (!feof(saveRealBoard))
        //{
        //    int numInputs = fscanf(saveRealBoard, "%c", realBoard);
        //    printf("Mindsweeper Board : \n");
        //    printf(realBoard);
        //}
        //fclose(saveRealBoard);

        // Create the Boards from the arrays with the newly scanned in files
        initialise(realBoard, myBoard);

        // Continue game as usual
        // i.e. Add functionality from playOnePlayerMinesweeperGame() to a new method that is called when the option
        // to load from a previous game is selected

    }
  
}

// method that saves game state to file
void saveGame()
{

    // Still left to be done -
    
    // Contain question in if statement in single player mode i.e. playOnePlayerMinesweeperGame()

    // After each turn ... Would you like to save a game? Y, N option, or on game exit if the player left early

    // This cannot be done if the game has ended or mine has been hit i.e. game over

    // write both boards to seperate files i.e. saveMyBoard.txt, saveRealBoard.txt
    
    // which will then be loaded using the  loadFromSavedGame()

}

// playOnePlayerMinesweeperGame method allows player to play one player game
void playOnePlayerMinesweeperGame()
{
    // Initially set the gameOver as false as it has just begun
    bool gameOver = false;

    // Set the size of Board
    SIDE = 8;

    // Set the number of mines for the game
    MINES = 10;

    // Actual Board and My Board
    char realBoard[MAXSIDE][MAXSIDE], myBoard[MAXSIDE][MAXSIDE];

    int movesLeft = SIDE * SIDE - MINES, x, y; // keeps a count of the moves left  (check for player winning)
    int mines[MAXMINES][2]; // stores the coordinates of all mines - x,y

    initialise(realBoard, myBoard);

    // setup the location of the random mines
    setupRandomMines(mines, realBoard);

    // Game keeps playing until a mine has been opened

    while (gameOver == false)
    {
        printf("Mindsweeper Board : \n");
        printMinesweeperBoard(myBoard);
        inputPlayersMove(&x, &y);

        gameOver = playMinesweeper(myBoard, realBoard, mines, x, y, &movesLeft);

        if ((gameOver == false) && (movesLeft == 0))
        {
            printf("\nYou've won Congratulations!\n");
            printf("\nYour final score was: %d ", finalScore);
            gameOver = true;
        }
        else if ((gameOver == true) && (movesLeft > 0))
        {
            printf("\nYou hit a Mine and Lost! Better luck next time!\n");
            printf("\nYour final score was: %d ", finalScore);
        }
    }

    return;
}

// playTwoPlayerMinesweeperGame method allows player to play two player game
void playTwoPlayerMinesweeperGame()
{
    // Initially set the gameOver as false as it has just begun
    bool gameOver = false;

    // Set the size of Board
    SIDE = 8;

    // Set the number of mines
    MINES = 10;

    // Actual Board and My Board
    char realBoard[MAXSIDE][MAXSIDE], myBoard[MAXSIDE][MAXSIDE];

    int movesLeft = SIDE * SIDE - MINES, x, y; // keeps a count of the moves left (check for player winning)
    int mines[MAXMINES][2]; // stores the coordinates of all mines - x,y

    initialise(realBoard, myBoard);

    // setup the location of the random mines
    setupRandomMines(mines, realBoard);

    // Game keeps playing until a mine has been opened

    int currentMoveCount = 1; // start at 1 so player one is selected

    // variables to count player scores
    int player1Score = 0;
    int player2Score = 0;

    while (gameOver == false)
    {
        currentMoveCount++;

        if (currentMoveCount % 2 == 0)
        {
            printf("Mindsweeper Board : \n");
            printMinesweeperBoard(myBoard);
            printf(" ******************PLAYER ONE MOVE ********************\n");
            inputPlayersMove(&x, &y);
            player1Score = finalScore - player2Score; 
            
        }
        else
        {
            printf("Mindsweeper Board : \n");
            printMinesweeperBoard(myBoard);
            printf("****************** PLAYER TWO MOVE ********************\n");
            inputPlayersMove(&x, &y);
            player2Score = finalScore - player1Score;
        }

        gameOver = playMinesweeper(myBoard, realBoard, mines, x, y, &movesLeft);

        if ((gameOver == false) && (movesLeft == 0))
        {
            if (currentMoveCount % 2 == 0)
            {
                printf("\nPlayer 1 you Won, Congratulations!\n");
                printf("\nPlayer 1's final score was: %d \n", player2Score);
                printf("\nPlayer 2's final score was: %d \n", player1Score);
            }
            else
            {
                printf("\nPlayer 2 you Won, Congratulations!\n");
                printf("\nPlayer 2's final score was: %d \n", player1Score);
                printf("\nPlayer 1's final score was: %d \n", player2Score);;
            }
            gameOver = true;
        }
        else if ((gameOver == true) && (movesLeft > 0))
        {
            if (currentMoveCount % 2 == 0)
            {
                printf("\nPlayer 1 you hit a Mine and Lost! Better luck next time!\n");
                printf("\nPlayer 1's final score was: %d \n", player2Score);
                printf("\nPlayer 2's final score was: %d \n", player1Score);
            }
            else
            {
                printf("\nPlayer 2 you hit a Mine and Lost! Better luck next time!\n");
                printf("\nPlayer 2's final score was: %d \n", player1Score);
                printf("\nPlayer 1's final score was: %d \n", player2Score);
            }
        }
    }
    return;
}

// chooseNumberOfPlayers method used to allow the player to choose the no. of players playing the game
void chooseNumberOfPlayers()
{
    int numberOfPlayers;

    printf("Enter the Number Of Players\n");
    printf("Press 1 for 1 Player Game \n");
    printf("Press 2 for 2 Player Game \n");

    scanf("%d", &numberOfPlayers);

    if (numberOfPlayers == 1)
    {
        // Player 1 Mode method
        playOnePlayerMinesweeperGame();
    }

    if (numberOfPlayers == 2)
    {
        // Player 2 Mode method
        playTwoPlayerMinesweeperGame();
    }

    return;
}

// runGame method used to start a new game or load a game saved
void runGame()
{
    int gameStatus;

    printf("Enter the desired Game Mode\n");
    printf("Press 1 for New Game \n");
    printf("Press 2 for Saved Game \n");

    scanf("%d", &gameStatus);

    if (gameStatus == 1)
    {
        // Method allows player to select a new 1 or 2 player game
        chooseNumberOfPlayers();
    }

    if (gameStatus == 2)
    {
        // Method allows user to load from a previously saved game
        loadFromSavedGame();
    }

    return;
}

// main method runs minesweeper game
int main()
{
    runGame();
    return (0);
}