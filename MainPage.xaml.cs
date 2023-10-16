namespace Mastermind;


public partial class MainPage : ContentPage
{
   
    public MainPage()
    {
        InitializeComponent();
        

    }

    async void Enter_Clicked(object sender, EventArgs e)
    {
        // Brings the player to a new game of master mind
        await Navigation.PushAsync(new Home());

    }

    private async void Rules_Button_Clicked(object sender, EventArgs e)
    {
        // Displays the rules to the player
        await DisplayAlert("Game Rules ", "To begin the game, click on any tile on the bottom row. \nChoose a colour and when all four tiles are assigned a colour click the <submit guess> button\n Black pegs tells you that you have a guess correct in correct position\n white pegs show that you have the right colour in the wrong position\n keep guessing until you get all the colours in the correct order\n N.B colours can be duplicated ", "continue");
    
    }

    private void Exit_Button_Clicked(object sender, EventArgs e)
    {
        // Exit out of the application
        Application.Current.Quit();

    }

    //******************** COULD NOT GET WORKING - ATTEMPTED SAVE METHODS BELOW  ***************************
    //********************************************************************************************************

    //   private void LoadGameState()
    //   {
    //       string fileText = "";
    //       // to write to a file, need a filename, need a stream writer
    //       string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

    //       string filename = Path.Combine(path, STATE_FILE);

    //       try       // do something, if it works then great.
    //       {
    //           using (var r = new StreamReader(filename))
    //           {
    //               fileText = r.ReadToEnd();
    //           }
    //       }
    //       catch  // catch the error and deal with it.
    //       {
    //           fileText = "There are no saved games, enjoy a new one on us...";
    //       }

    //       FileInfo file = new FileInfo(filename);
    //       file.Delete();

    //       LblStatus.Text = fileText;
    //   }
}
