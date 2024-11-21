using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Library
{
    public class FileHandler2
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

    public class CoinCalculator
    {
        // Метод для обчислення мінімальної кількості монет
        public int GetMinCoins(int[] coins, int K)
        {
            int[] dp = new int[K + 1];
            Array.Fill(dp, K + 1);
            dp[0] = 0;

            foreach (var coin in coins)
            {
                for (int i = coin; i <= K; i++)
                {
                    dp[i] = Math.Min(dp[i], dp[i - coin] + 1);
                }
            }

            return dp[K] > K ? -1 : dp[K];
        }
    }

    public static class Lab2
    {
        public static void Run(string inputFilePath, string outputFilePath)
        {
            Console.WriteLine($"Input File Path: {inputFilePath}");
            Console.WriteLine($"Output File Path: {outputFilePath}");

            var fileHandler = new FileHandler2();
            var coinCalculator = new CoinCalculator();

            try
            {
                // Читання та валідація вхідних даних
                var (N, coins, K) = fileHandler.ReadInput(inputFilePath);

                // Обчислення мінімальної кількості монет
                int result = coinCalculator.GetMinCoins(coins, K);

                // Запис результату у вихідний файл
                fileHandler.WriteOutput(outputFilePath, result);

                Console.WriteLine("Lab 2 executed successfully.");
            }
            catch (InvalidDataException ex)
            {
                Console.WriteLine($"Invalid data: {ex.Message}");
                fileHandler.WriteOutput(outputFilePath, -1);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                fileHandler.WriteOutput(outputFilePath, -1);
            }
        }
    }
}