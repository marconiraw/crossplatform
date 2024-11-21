// Lab2Tests.cs
using Xunit;

namespace Lab2.Tests
{
    public class Lab2Tests
    {
        private readonly CoinCalculator coinCalculator;

        public Lab2Tests()
        {
            coinCalculator = new CoinCalculator();
        }

        [Theory]
        [InlineData(new int[] { 4, 9 }, 2, -1)]
        [InlineData(new int[] { 1, 3, 4 }, 6, 2)]
        [InlineData(new int[] { 2, 5, 10 }, 11, 2)]
        [InlineData(new int[] { 1 }, 0, 0)]
        [InlineData(new int[] { 1 }, 1, 1)]
        [InlineData(new int[] { 2 }, 3, -1)]
        [InlineData(new int[] { 1, 2, 5 }, 11, 3)]
        [InlineData(new int[] { 5, 7, 1 }, 12, 2)] // 7 + 5
        [InlineData(new int[] { 3, 5, 7 }, 2, -1)]
        [InlineData(new int[] { 2, 4, 6 }, 8, 2)] // 4 + 4
        public void GetMinCoins_Test(int[] coins, int K, int expected)
        {
            // Act
            int result = coinCalculator.GetMinCoins(coins, K);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
