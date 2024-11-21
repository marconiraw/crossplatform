using System;
using System.IO;
using System.Linq;

namespace Lab2
{
    public class FileHandler
    {
        // Метод для читання та валідації вхідних даних
        public (int N, int[] Coins, int K) ReadInput(string inputFilePath)
        {
            if (!File.Exists(inputFilePath))
            {
                throw new FileNotFoundException("Input file does not exist.");
            }

            var lines = File.ReadAllLines(inputFilePath);
            if (lines.Length < 3)
            {
                throw new InvalidDataException("Input file must contain at least 3 lines.");
            }

            bool parseN = int.TryParse(lines[0].Trim(), out int N);
            bool parseK = int.TryParse(lines[2].Trim(), out int K);

            if (!parseN || !parseK || N <= 0)
            {
                throw new InvalidDataException("Invalid values for N or K.");
            }

            var coinsStr = lines[1].Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (coinsStr.Length != N)
            {
                throw new InvalidDataException("Number of coins does not match N.");
            }

            bool parseCoins = coinsStr.All(s => int.TryParse(s, out _));
            if (!parseCoins)
            {
                throw new InvalidDataException("All coins must be integers.");
            }

            int[] coins = coinsStr.Select(int.Parse).ToArray();

            return (N, coins, K);
        }

        // Метод для запису результату у вихідний файл
        public void WriteOutput(string outputFilePath, int result)
        {
            File.WriteAllText(outputFilePath, result.ToString());
        }
    }
}
