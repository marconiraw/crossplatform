using System;

namespace Lab2
{
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
}
