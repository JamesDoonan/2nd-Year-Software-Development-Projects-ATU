using System;
using System.Diagnostics;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Newtonsoft.Json;
using System.Reflection;

namespace Mastermind;

// Attempt at saving current position and guesses
//public class GameState
//{
//    public int[] playerMove;
//    bool gameWon;
//    List<String> playerCurrentGuess;
//    List<String> secretSequence;
//    Color[] _guessedSequenceFromButtonColours;


//    public GameState()
//    {
//        playerMove = new int[1];
//        gameWon = false;
//        playerCurrentGuess = new List<String>();
//        secretSequence = new List<String>();
//        _guessedSequenceFromButtonColours = new Color[4];
//    }
//}

public partial class Home : ContentPage
{
    private Random _random;
    private Button currentGuessPeg;

    // Game Status
    bool gameWon = false;
    int playerMove = 0;
    string secretSequence;

    // Colour Arrays for guessed sequence & hints
    Color[] _guessedSequenceFromButtonColours = new Color[4];
    Color[] _hintColours = new Color[4];

    // Lists for the secret colour sequence, the player guessed sequence and the evaluation of these
    List<String> _secretColourSequenceInHex = new List<String>();
    List<String> _secretColourSequence = new List<String>();
    List<String> _guessedColourSequence = new List<String>(); // _guessedSequenceFromButtonColours converted to string hex values for evaluation
    List<String> _hintColourSequence = new List<String>();
    List<Boolean> _guessEvaluation = new List<Boolean>();

    // Save to file
   public string filename = "SaveMinesweeper.txt";
   // private GameState _gameState;

    public Home()
    {
        InitializeComponent();
        initializeGame();

    }

    public void initializeGame()
    {
        // Create a new random
        _random = new Random();

        // Reset secret colour seq list thats revealed to the player at the begining
        _secretColourSequence = new List<String>();

        // Create the secret sequence for the player
        createSecretColourSequence();

    }

    // Buttons Functions
    private void Button_Clicked(object sender, EventArgs e)
    {
        if (currentGuessPeg != null)
            currentGuessPeg.BorderColor = Colors.Transparent;


        currentGuessPeg = (Button)sender;
        currentGuessPeg.BorderColor = Colors.Magenta;
        GridColourPicker.IsVisible = true;
    }

    async void Exit_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());

    }

    private void MovePiece(Button BtnGuess, BoxView hintColour)
    {
        // If the game hasn't been won and the guess is incorrect
        // Move the guess selection buttons up a row for the player to go again
        MoveUp(BtnGuess, hintColour);
    }

    private void retainPreviousRowsGuessedColourSequence(Color[] _guessedSequenceFromButtonColours, int row)
    {
        int gridRow = row;
        int gridColumn = 0;

        for (int i = 0; i < _guessedSequenceFromButtonColours.Length; i++)
        {

            // Create a new box view and set the guessed colour
            BoxView newBoxViewGuessedSequence = new BoxView
            {
                Color = _guessedSequenceFromButtonColours[i],
                HeightRequest = 6,
                WidthRequest = 6,
                CornerRadius = 5,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            // Add new box view to the grid (grid.Add(newBoxView, columnNumber, rowNumber)
            GridGameTable.Add(newBoxViewGuessedSequence, gridColumn, gridRow);

            gridColumn++;

        }
    }

    private void retainPreviousRowsHints(Color[] _hintColours, int row)
    {
        int gridRow = row;
        int gridColumn = 0;

        for (int i = 0; i < _hintColours.Length; i++)
        {

            // Create a new box view and set the hint colour
            BoxView newBoxViewHints = new BoxView
            {
                Color = _hintColours[i],
                HeightRequest = 6,
                WidthRequest = 6,
                CornerRadius = 3,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            // Add new box view to the grid (grid.Add(newBoxView, columnNumber, rowNumber)
            GridGameHintTable.Add(newBoxViewHints, gridColumn, gridRow);

            gridColumn++;

        }

    }

    private void MoveUp(Button button, BoxView hintBoxView)
    {
        // Get the row value from either button or boxview as this'll be the same
        int row = (int)button.GetValue(Grid.RowProperty);

        // Before the row changes

        // Set the previous blocks row of colours to what the inputted guess was
        retainPreviousRowsGuessedColourSequence(_guessedSequenceFromButtonColours, row);

        // Set the previous blocks row of hints to what the hints were
        retainPreviousRowsHints(_hintColours, row);

        // Minus 1, pushing the game up a row
        row--;

        // Set new row value for both buttons and box views
        button.SetValue(Grid.RowProperty, row);
        hintBoxView.SetValue(Grid.RowProperty, row);

        // Reset the colours of the button once the row has moved up
        BtnGuess1.BackgroundColor = Colors.Transparent;
        BtnGuess2.BackgroundColor = Colors.Transparent;
        BtnGuess3.BackgroundColor = Colors.Transparent;
        BtnGuess4.BackgroundColor = Colors.Transparent;

        // Reset the colours of the hints once the row has moved up
        Hint1.BackgroundColor = Colors.BlueViolet;
        Hint2.BackgroundColor = Colors.BlueViolet;
        Hint3.BackgroundColor = Colors.BlueViolet;
        Hint4.BackgroundColor = Colors.BlueViolet;

        // Reset all necessary Lists & Arrays
        _guessedSequenceFromButtonColours = new Color[4];
        _hintColours = new Color[4];
        _guessedColourSequence = new List<String>();
        _hintColourSequence = new List<String>();
        _guessEvaluation = new List<Boolean>();

    }

    private void ColorGuessBoxView_Tapped(object sender, EventArgs e)
    {
        BoxView b = (BoxView)sender;

        currentGuessPeg.BackgroundColor = b.Color;
        currentGuessPeg.BorderColor = Colors.Transparent;
        GridColourPicker.IsVisible = false;

    }

    private async void Guess_Clicked(object sender, EventArgs e)
    {
        // Get and Set the colours selected by the player depending on index from our BtnGuess background colours
        // if they've been inputted by the player. There is a check for this below
        _guessedSequenceFromButtonColours[0] = BtnGuess1.BackgroundColor;
        _guessedSequenceFromButtonColours[1] = BtnGuess2.BackgroundColor;
        _guessedSequenceFromButtonColours[2] = BtnGuess3.BackgroundColor;
        _guessedSequenceFromButtonColours[3] = BtnGuess4.BackgroundColor;

        // Test Scenario:
        // Disable the submit guess button if the _guessedSequenceFromButtonColours array is empty or the default colour
        // In this case no guesses by the player would have been made and the button was clicked prematurely
        if (BtnGuess1.BackgroundColor == Colors.Transparent || BtnGuess2.BackgroundColor == Colors.Transparent || BtnGuess3.BackgroundColor == Colors.Transparent ||
            BtnGuess4.BackgroundColor == Colors.Transparent)
        {
            BtnSubmit.IsEnabled = false;

            // Display message of the player error
            await DisplayAlert("Uh Oh! Theres been an Error! ", "Guess buttons not populated!", "Try Again");

            // Then re_enable button
            BtnSubmit.IsEnabled = true;

            // Exit back out of the Guesss_Clicked method so the player can try again

            return;
        }
        else
        {
            BtnSubmit.IsEnabled = true;
        }

        // Increment the move the player is on - this is a check used later to determine the number of tries (i.e. 10 rows)
        // And a determiner for whether to contunue the game
        playerMove++;

        // Convert the colour array to a list of string hex value of colours for later processing
        convertColourGuessedToText(_guessedSequenceFromButtonColours);

        // Check this guessed sequence against the randomly generated secret sequence
        // thus also setting whether the gameWon is true/false
        checkGuessedSequence(_secretColourSequence, _guessedColourSequence, _hintColours);

        // If the guess was wrong (game not been won) & the player still has go's left (rows left to guess)
        if ((!gameWon) && (playerMove <= 9))
        {
            // Test Scenario:
            // If the Test inputted by the player is wrong, wrong guess display alert is shown
            await DisplayAlert("Guess # " + playerMove, "Wrong Guess, Guess Again!", "Continue");

            // Will only move the game up a row if the game hasn't been won, i.e the guess was incorrect and the row is greater that or equal to zero
            MovePiece(BtnGuess1, Hint1);
            MovePiece(BtnGuess2, Hint2);
            MovePiece(BtnGuess3, Hint3);
            MovePiece(BtnGuess4, Hint4);
        }
        else if ((!gameWon) && (playerMove >= 10))
        {
            // The player loses
            // Display secret sequence to player
            await DisplayAlert("Game over!", "You guessed incorrectly!!\n " + secretSequence, "Exit");

            // Back to Menu
            await Navigation.PushAsync(new MainPage());
        }
        else if (gameWon)
        {
            // The player wins
            // Display secret sequence to player
            await DisplayAlert("Congratulations", "You guessed correctly!\n " + secretSequence, "Exit");

            // Back to Menu
            await Navigation.PushAsync(new MainPage());
        }
        else
        {
            // Error
            // Display secret sequence to player
            await DisplayAlert("Uh oh! ", "There's been an error!\n " + secretSequence, "Exit");

            // Back to Menu
            await Navigation.PushAsync(new MainPage());
        }

    }

    // Game Rule Function Methods
    private List<String> convertColourGuessedToText(Color[] _guessedSequenceFromButtonColours)
    {
        int i;
        string colourGuessed;

        for (i = 0; i < 4; i++)
        {
            string colour;
            colourGuessed = _guessedSequenceFromButtonColours[i].ToHex();

            switch (colourGuessed)
            {
                case "#FF0000":
                    colour = "Red";
                    break;

                case "#A020F0":
                    colour = "Purple";
                    break;

                case "#FFFF00":
                    colour = "Yellow";
                    break;

                case "#0000FF":
                    colour = "Blue";
                    break;

                default:
                    colour = " ";
                    break;
            }

            _guessedColourSequence.Add(colour);

        }

        return _guessedColourSequence;

    }


    private void createSecretColourSequence()
    {
        // Hex numbers for colours in order { Red, Purple, Yellow, Blue}
        List<String> _secretColours = new List<String> { "#FF0000", "#A020F0", "#FFFF00", "#0000FF" };
        _secretColourSequenceInHex.Add(_secretColours[_random.Next(0, 3)]);
        _secretColourSequenceInHex.Add(_secretColours[_random.Next(0, 3)]);
        _secretColourSequenceInHex.Add(_secretColours[_random.Next(0, 3)]);
        _secretColourSequenceInHex.Add(_secretColours[_random.Next(0, 3)]);

        // Test Scenario: uncomment below to test a specific set of colours as the winning sequence to confirm all is working correctly
        //_secretColourSequenceInHex.Add("#FF0000"); // Red
        //_secretColourSequenceInHex.Add("#FF0000"); // Red
        //_secretColourSequenceInHex.Add("#FF0000"); // Red
        //_secretColourSequenceInHex.Add("#FF0000"); // Red

        // Add secret colour sequence to a new variable to convert the hex values to string colours the player can understand
        // ( If I had more time I would have created a new seperate method with the logic from this and convertColourGuessedToText() method extracted
        for (int i = 0; i < 4; i++)
        {
            string colour;
            string hexValueToStringColour;
            hexValueToStringColour = _secretColourSequenceInHex[i];

            switch (hexValueToStringColour)
            {
                case "#FF0000":
                    colour = "Red";
                    break;

                case "#A020F0":
                    colour = "Purple";
                    break;

                case "#FFFF00":
                    colour = "Yellow";
                    break;

                case "#0000FF":
                    colour = "Blue";
                    break;

                default:
                     colour = " ";
                    break;
            }

            _secretColourSequence.Add(colour);

        }

        secretSequence = string.Format("The Secret Sequence was: {0} ", string.Join(", ", _secretColourSequence));

        // Test Scenario:  Log the values of the list to see if the secrets been populated correctly String Hex Number
        //testScenarios(secretSequence);

    }

    // TEST SCENARIO METHOD (mentioned in excel testing file)
    //private async void testScenarios(String testCorrectListReturned)
    //{
    //    await DisplayAlert("Test Scenario", "Secret Colour Returned\n " + testCorrectListReturned, "Exit");
    //}

    // This method checks the secret colour sequence and compares this to the players guess. It returns a colour array where by if there is an exact match
    //  a black pin returned at that index, a non-exact match is a white pin, and a colour not present in the secret seq returns a blue violet colour (same as the background of the grid)
    private Color[] checkGuessedSequence(List<String> _secretColourSequence, List<String> _guessedColourSequence, Color[] _hintColours)
    {
        // Create duplicates lists of our secret & guessed sequences (List string hex colours) so we don't alter the original lists
        List<String> _checkSecretSeq = new List<String>();
        for (int colourIndex = 0; colourIndex < _secretColourSequence.Count; colourIndex++)
        {
            _checkSecretSeq.Add(new(_secretColourSequence[colourIndex]));
        }

        List<String> _checkGuessedSeq = new List<String>();
        for (int colourIndex = 0; colourIndex < _guessedColourSequence.Count; colourIndex++)
        {
            _checkGuessedSeq.Add(new(_guessedColourSequence[colourIndex]));
        }

        // Firstly check for all the exact matches
        for (int i = 0; i < _checkGuessedSeq.Count; i++)
        {
            // Check if there is an exact match secretSeq matches the string colour in the players guessed seq
            if (_checkSecretSeq[i].Equals(_checkGuessedSeq[i]))
            {
                // If this is the case set colour in the Colour array to black at this index
                _hintColours[i] = Colors.Black;

                // Continue to next index i
                continue;
            }
        }

        // Secondly check for all non-exact matches
        for (int j = 0; j < _checkGuessedSeq.Count; j++)
        {
            // Check if a hint has occured at this index of the list already? if so skip this index and continue to the next
            if (_hintColours[j] == Colors.Black)
            {
                // i.e. Guess has already been mapped - move on to the next index
                continue;
            }

            // Next find the index where colours in the secret seqeuence are similar to those in the guessed seq
            // but might not be in the correct index position
            int index = _checkSecretSeq.FindIndex(colour => colour == _checkGuessedSeq[j]);

            // If non are found from the FindIndex statement above, set the hint as BlueViolet (same colour as the grid so as not to confuse the player)
            // NB: an index result of -1 means not found
            if (index == -1)
            {
                // If nothing was found, set the colour to BlueViolet
                _hintColours[j] = Colors.BlueViolet;

                // Move on to the next index
                continue;
            }

            // If neither the hint colour is an exact match i.e. colour set to black
            // And the hint colour is not a colour present in the secret seq i.e colour set to BlueViolet
            if (!(_hintColours[j] == Colors.Black) || !(_hintColours[j] == Colors.BlueViolet))
            {
                // Set the index to white
                _hintColours[j] = Colors.White;
            }

        }

        // Update the game status variables based off of the hints
        checkCorrectGuess(_hintColours, _hintColourSequence);

        return _hintColours;

    }

    private bool checkCorrectGuess(Color[] _hintColours, List<String> _hintColourSequence)
    {
        // Convert _hintColours array to string List of hex colours of black and white
        gameWon = false;
        string hintColour;

        for (int i = 0; i < 4; i++)
        {
            string colour;
            hintColour = _hintColours[i].ToHex();

            switch (hintColour)
            {
                case "#FFFFFF":
                    colour = "White";
                    break;

                case "#000000":
                    colour = "Black";
                    break;

                case "#8A2BE2":
                    colour = "BlueViolet";
                    break;

                default:
                    colour = "Error";
                    break;
            }

            _hintColourSequence.Add(colour);

        }

        // Determine and set all of the guessEvaluations from the hintColourSequence of black and white hex colours
        for (int i = 0; i < _hintColourSequence.Count; i++)
        {
            if (_hintColourSequence[i].Equals("Black")) // Check if each index is equal to string value of "Black"
            {
                _guessEvaluation.Add(true);

            }
            else if (_hintColourSequence[i].Equals("White")) // Check if each index is equal to string value of "White"
            {
                _guessEvaluation.Add(false);

            }
            else if (_hintColourSequence[i].Equals("BlueViolet")) // Check if each index is equal to string value of "BlueViolet"
            {
                _guessEvaluation.Add(false);

            }
            else if (_hintColourSequence[i].Equals("Error"))    // Check for an Error
            {
                _guessEvaluation.Add(false);

            }
        }

        // Check if the guessEvaluations list does not contain false
        if (!(_guessEvaluation.Contains(false)))
        {
            // If this is the case all guesses at each index are correct and the players guess was true - the player wins the game!
            gameWon = true;
        }
        else
        {
            // If this is not the case - the player loses!
            gameWon = false;
            setHintColours(_hintColours);

        }

        return gameWon;

    }

    private void setHintColours(Color[] _hintColours)
    {
        // Set the colour of the BoxView hints to help the player using the values at each index of the _hintColours array
        Hint1.BackgroundColor = _hintColours[0];
        Hint2.BackgroundColor = _hintColours[1];
        Hint3.BackgroundColor = _hintColours[2];
        Hint4.BackgroundColor = _hintColours[3];

    }

    //******************** COULD NOT GET WORKING - ATTEMPTED SAVE METHODS BELOW  ***************************
    //********************************************************************************************************
    //private GameState ReadJsonFile()
    //{
    //    GameState gs = new GameState();
    //    string jsonText = "";

    //    try     // reading the local directory (environment.specialfolders)
    //    {
    //        string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    //        string fname = Path.Combine(path, filename);
    //        using (var reader = new StreamReader(fname))
    //        {
    //            jsonText = reader.ReadToEnd();
    //        }
    //    }
    //    catch (Exception ex)    // if that fails, then read the embedded resource
    //    {
    //        string errorMsg = ex.Message;
    //    }   // end try catch
    //    if (jsonText != "")
    //    {
    //        gs = new GameState();
    //        gs = JsonConvert.DeserializeObject<GameState>(jsonText);
    //        return gs;
    //    }
    //    else
    //        return null;
    //}

    //public void SaveListOfData(GameState gs)
    //{
    //    string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

    //    string fname = System.IO.Path.Combine(path, filename);

    //    using (var writer = new StreamWriter(fname, false))
    //    {
    //        string jsonText = JsonConvert.SerializeObject(gs);
    //        writer.WriteLine(jsonText);
    //    }
    //}

    //private void BtnReadFile_Clicked(object sender, EventArgs e)
    //{
    //    string fileContent = "";
    //    _gameState = ReadJsonFile();

    //    if (_gameState != null)
    //    {
    //        fileContent = "Row values: " + _gameState.playerRows[1];
    //        fileContent = "Current Guess: " + _gameState.playerCurrentGuess[1];
    //    }
    //    else
    //    {
    //        fileContent = "No file found";
    //    }
    //          LblFileStuff.Text = fileContent;

    //}

    //public void BtnSave_Clicked(object sender, EventArgs e)
    //{
    //    GameState gs = new GameState();
    //    gs.playerRows[1] = gridRow;
    //    gs.playerRows[1] = playerMove;
    //    gs.playerCurrentGuess[] = _guessedColourSequence[];
    //    gs.playerCurrentHints[] = _hintColours[];

    //    SaveListOfData(gs);
    //}

}
