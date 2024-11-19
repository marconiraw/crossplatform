using System;

namespace CardGame
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Create a file handler instance with the input file name
                var fileHandler = new FileHandler("INPUT.TXT");
                // Read and validate data from the input file
                fileHandler.ReadAndValidate();

                // Create an instance of the card game logic
                var cardGame = new CardGameLogic(fileHandler.PlayerCards, fileHandler.AttackingCards, fileHandler.TrumpSuit);
                // Determine if the player can defend against the attacking cards
                string result = cardGame.CanDefend() ? "YES" : "NO";
                // Write the result to the output file
                fileHandler.WriteResult(result);
            }
            catch (ArgumentException ex)
            {
                // If any validation error occurs, write the error message to the output file
                File.WriteAllText("OUTPUT.TXT", ex.Message);
            }
        }
    }
}
