// Lab3Tests.cs
using Xunit;
using System.Collections.Generic;

namespace Lab3.Tests
{
    public class Lab3Tests
    {
        private readonly ArborescenceCalculator calculator;

        public Lab3Tests()
        {
            calculator = new ArborescenceCalculator();
        }

        [Fact]
        public void TestSampleInput()
        {
            // Arrange
            int n = 3;
            List<Edge> edges = new List<Edge>
            {
                new Edge(1, 2, 5, 1),
                new Edge(1, 3, 10, 2),
                new Edge(3, 2, 4, 3)
            };

            // Act
            var result = calculator.GetMinimumArborescence(n, edges, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(9, result.TotalCost); // Канали 1 та 3: 5 + 4
            Assert.Contains(1, result.ChannelIndices);
            Assert.Contains(3, result.ChannelIndices);
            Assert.Equal(2, result.ChannelIndices.Count);
        }

        [Fact]
        public void TestNoChannels()
        {
            // Arrange
            int n = 1;
            List<Edge> edges = new List<Edge>();

            // Act
            var result = calculator.GetMinimumArborescence(n, edges, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.TotalCost);
            Assert.Empty(result.ChannelIndices);
        }

        [Fact]
        public void TestDisconnectedGraph()
        {
            // Arrange
            int n = 4;
            List<Edge> edges = new List<Edge>
            {
                new Edge(1, 2, 3, 1),
                new Edge(2, 3, 4, 2)
                // Місто 4 не підключене
            };

            // Act
            var result = calculator.GetMinimumArborescence(n, edges, 1);

            // Assert
            Assert.Null(result); // Неможливо досягти міста 4
        }

        [Fact]
        public void TestMultipleChannelsBetweenCities()
        {
            // Arrange
            int n = 4;
            List<Edge> edges = new List<Edge>
            {
                new Edge(1, 2, 1, 1),
                new Edge(1, 2, 2, 2),
                new Edge(2, 3, 3, 3),
                new Edge(3, 4, 4, 4),
                new Edge(1, 4, 10, 5)
            };

            // Act
            var result = calculator.GetMinimumArborescence(n, edges, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(8, result.TotalCost); // Канали 1, 3, 4: 1 + 3 + 4
            Assert.Contains(1, result.ChannelIndices);
            Assert.Contains(3, result.ChannelIndices);
            Assert.Contains(4, result.ChannelIndices);
            Assert.Equal(3, result.ChannelIndices.Count);
        }

        [Fact]
        public void TestSelfLoopIgnored()
        {
            // Arrange
            int n = 3;
            List<Edge> edges = new List<Edge>
            {
                new Edge(1, 2, 2, 1),
                new Edge(2, 2, 3, 2), // Самозв'язок, має бути ігнорований
                new Edge(2, 3, 4, 3)
            };

            // Act
            var result = calculator.GetMinimumArborescence(n, edges, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.TotalCost); // Канали 1 та 3: 2 + 4
            Assert.Contains(1, result.ChannelIndices);
            Assert.Contains(3, result.ChannelIndices);
            Assert.Equal(2, result.ChannelIndices.Count);
        }
    }
}
