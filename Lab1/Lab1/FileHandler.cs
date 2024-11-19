using System;
using System.IO;
using System.Text;
using System.Linq;

namespace CardGame
{
    public class FileHandler
    {
        public int N { get; private set; }
        public int M { get; private set; }
        public char TrumpSuit { get; private set; }
        public string[] PlayerCards { get; private set; } // Make non-nullable
        public string[] AttackingCards { get; private set; } // Make non-nullable

        private readonly string inputFile;
        private readonly string outputFile = "OUTPUT.TXT";

        public FileHandler(string inputFile)
        {
            this.inputFile = inputFile;
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
}
