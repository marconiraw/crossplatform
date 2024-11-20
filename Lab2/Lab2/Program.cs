using System;
using System.IO;
using System.Linq;

namespace Lab2
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Шляхи до файлів input.txt та output.txt
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Path.GetFullPath(Path.Combine(baseDirectory, "..", "..", "..", ".."));
            string inputFile = Path.Combine(projectDirectory, "input.txt");
            string outputFile = Path.Combine(projectDirectory, "output.txt");

            if (!File.Exists(inputFile))
            {
                File.WriteAllText(outputFile, "-1");
                return;
            }

            var lines = File.ReadAllLines(inputFile);
            if (lines.Length < 3)
            {
                File.WriteAllText(outputFile, "-1");
                return;
            }

            bool parseN = int.TryParse(lines[0].Trim(), out int N);
            bool parseK = int.TryParse(lines[2].Trim(), out int K);

            if (!parseN || !parseK || N <= 0)
            {
                File.WriteAllText(outputFile, "-1");
                return;
            }

            var coinsStr = lines[1].Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (coinsStr.Length != N)
            {
                File.WriteAllText(outputFile, "-1");
                return;
            }

            bool parseCoins = coinsStr.All(s => int.TryParse(s, out _));
            if (!parseCoins)
            {
                File.WriteAllText(outputFile, "-1");
                return;
            }

            int[] coins = coinsStr.Select(int.Parse).ToArray();

            int result = GetMinCoins(coins, K);
            File.WriteAllText(outputFile, result.ToString());
        }

        public static int GetMinCoins(int[] coins, int K)
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
}
