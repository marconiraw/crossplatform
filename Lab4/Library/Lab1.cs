using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Library
{
    public class FileHandler
    {
        public int N { get; private set; }
        public int M { get; private set; }
        public char TrumpSuit { get; private set; }
        public string[] PlayerCards { get; private set; } // Make non-nullable
        public string[] AttackingCards { get; private set; } // Make non-nullable

        private readonly string inputFile;
        private readonly string outputFile;

        public FileHandler(string inputFile, string outputFile)
        {
            this.inputFile = inputFile;
            this.outputFile = outputFile;
        }

        // Reads and validates input data from the file
        public void ReadAndValidate()
        {
            string[] lines = File.ReadAllLines(inputFile, Encoding.UTF8);

            if (lines.Length != 3)
            {
                throw new ArgumentException("Input file must contain exactly 3 lines.");
            }

            string[] firstLine = lines[0].Split();
            if (firstLine.Length != 3)
            {
                throw new ArgumentException("The first line must contain 3 values: N, M, and the trump suit.");
            }

            // Use local variables for parsing
            if (!int.TryParse(firstLine[0], out int n) || n < 1 || n > 35)
            {
                throw new ArgumentException("N must be an integer between 1 and 35.");
            }

            if (!int.TryParse(firstLine[1], out int m) || m < 1 || m > 4 || m > n)
            {
                throw new ArgumentException("M must be an integer between 1 and 4, and M <= N.");
            }

            // Assign the validated values to the properties
            N = n;
            M = m;

            // Validate trump suit
            if (firstLine[2].Length != 1 || !"SCDH".Contains(firstLine[2]))
            {
                throw new ArgumentException("Trump suit must be one of the following: 'S', 'C', 'D', 'H'.");
            }
            TrumpSuit = firstLine[2][0];

            // Parse and validate player cards
            PlayerCards = lines[1].Split();
            if (PlayerCards.Length != N)
            {
                throw new ArgumentException($"Player must have exactly {N} cards.");
            }
            ValidateCards(PlayerCards);

            // Parse and validate attacking cards
            AttackingCards = lines[2].Split();
            if (AttackingCards.Length != M)
            {
                throw new ArgumentException($"There must be exactly {M} attacking cards.");
            }
            ValidateCards(AttackingCards);
        }

        // Writes the result to the output file
        public void WriteResult(string result)
        {
            File.WriteAllText(outputFile, result);
        }

        // Validates a set of cards
        private void ValidateCards(string[] cards)
        {
            foreach (string card in cards)
            {
                if (card.Length != 2 || !"6789TJQKA".Contains(card[0]) || !"SCDH".Contains(card[1]))
                {
                    throw new ArgumentException($"Invalid card format: {card}. Cards must have a valid rank and suit.");
                }
            }
        }
    }

    public class CardGameLogic
    {
        private readonly string[] playerCards;
        private readonly string[] attackingCards;
        private readonly char trumpSuit;

        public CardGameLogic(string[] playerCards, string[] attackingCards, char trumpSuit)
        {
            this.playerCards = playerCards;
            this.attackingCards = attackingCards;
            this.trumpSuit = trumpSuit;
        }

        // Determines if the player can defend against the attacking cards
        public bool CanDefend()
        {
            foreach (var attackCard in attackingCards)
            {
                bool canDefend = false;

                // Check if there's a higher card of the same suit or a trump card
                foreach (var playerCard in playerCards)
                {
                    if (IsStrongerCard(attackCard, playerCard))
                    {
                        canDefend = true;
                        break;
                    }
                }

                // If no defense was found for this attack card, return false
                if (!canDefend)
                {
                    return false;
                }
            }
            return true; // All attack cards can be defended
        }

        // Determines if a player's card can defend against an attack card
        private bool IsStrongerCard(string attackCard, string playerCard)
        {
            char attackRank = attackCard[0];
            char playerRank = playerCard[0];
            char attackSuit = attackCard[1];
            char playerSuit = playerCard[1];

            // If the attack card is a trump card
            if (attackSuit == trumpSuit)
            {
                return playerSuit == trumpSuit && GetRankValue(playerRank) > GetRankValue(attackRank);
            }
            else
            {
                // For non-trump cards
                return (playerSuit == attackSuit && GetRankValue(playerRank) > GetRankValue(attackRank)) ||
                       (playerSuit == trumpSuit);
            }
        }

        // Gets the value of a rank for comparison
        private int GetRankValue(char rank)
        {
            return rank switch
            {
                '6' => 1,
                '7' => 2,
                '8' => 3,
                '9' => 4,
                'T' => 5,
                'J' => 6,
                'Q' => 7,
                'K' => 8,
                'A' => 9,
                _ => 0,
            };
        }
    }

    public static class Lab1
    {
        public static void Run(string inputFilePath, string outputFilePath)
        {
            Console.WriteLine($"Input File Path: {inputFilePath}");
            Console.WriteLine($"Output File Path: {outputFilePath}");

            try
            {
                // Create a file handler instance with the input file name
                var fileHandler = new FileHandler(inputFilePath, outputFilePath);
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
                File.WriteAllText("output.txt", ex.Message);
            }
        }
    }
}
