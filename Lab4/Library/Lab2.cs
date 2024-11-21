using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Library
{
    public static class FileManager
    {
        public InputData ReadInput(string inputFilePath)
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

            return new InputData
            {
                N = N,
                Coins = coins,
                K = K
            };
        }

        public void WriteOutput(string outputFilePath, int result)
        {
            File.WriteAllText(outputFilePath, result.ToString());
        }
    }

    public class InputData
    {
        public int N { get; set; }
        public int[] Coins { get; set; }
        public int K { get; set; }
    }

    public static class CoinCalculator
    {
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

            try
            {
                // Чтение и валидация входных данных
                InputData inputData = FileManager.ReadInput(inputFilePath);

                // Вычисление минимального количества монет
                int result = CoinCalculator.GetMinCoins(inputData.Coins, inputData.K);

                // Запись результата в выходной файл
                FileManager.WriteOutput(outputFilePath, result);

                Console.WriteLine("Lab 2 executed successfully.");
            }
            catch (InvalidDataException ex)
            {
                Console.WriteLine($"Invalid data: {ex.Message}");
                FileManager.WriteOutput(outputFilePath, -1);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                FileManager.WriteOutput(outputFilePath, -1);
            }
        }
    }
}