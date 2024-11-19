using System;
using System.IO;
using Xunit;

namespace CardGame.Tests
{
	public class FileHandlerTests
	{
		private const string inputFilePath = "INPUT.TXT";
		private const string outputFilePath = "OUTPUT.TXT";

		// Test for valid input data
		[Fact]
		public void ReadAndValidate_ValidInput_ReturnsCorrectValues()
		{
			// Arrange
			var lines = new[]
			{
				"5 3 S", // N=5, M=3, Trump suit='S'
                "6S 7H 8D 9C TH",
				"7S 8C 9D"
			};
			File.WriteAllLines(inputFilePath, lines);

			var fileHandler = new FileHandler(inputFilePath);

			// Act
			fileHandler.ReadAndValidate();

			// Assert
			Assert.Equal(5, fileHandler.N);
			Assert.Equal(3, fileHandler.M);
			Assert.Equal('S', fileHandler.TrumpSuit);
			Assert.Equal(new[] { "6S", "7H", "8D", "9C", "TH" }, fileHandler.PlayerCards);
			Assert.Equal(new[] { "7S", "8C", "9D" }, fileHandler.AttackingCards);

			// Cleanup
			File.Delete(inputFilePath);
		}

		// Test for invalid data (non-integer values for N or M)
		[Fact]
		public void ReadAndValidate_InvalidData_ThrowsArgumentException()
		{
			// Arrange
			var lines = new[] { "five three S", "6S 7H", "7S" };
			File.WriteAllLines(inputFilePath, lines);
			var fileHandler = new FileHandler(inputFilePath);

			// Act & Assert
			var exception = Assert.Throws<ArgumentException>(() => fileHandler.ReadAndValidate());
			Assert.Equal("N must be an integer between 1 and 35.", exception.Message);

			// Cleanup
			File.Delete(inputFilePath);
		}

		// Test for incorrect number of lines
		[Fact]
		public void ReadAndValidate_IncorrectNumberOfLines_ThrowsArgumentException()
		{
			// Arrange
			var lines = new[] { "5 3 S" }; // Only one line
			File.WriteAllLines(inputFilePath, lines);
			var fileHandler = new FileHandler(inputFilePath);

			// Act & Assert
			var exception = Assert.Throws<ArgumentException>(() => fileHandler.ReadAndValidate());
			Assert.Equal("Input file must contain exactly 3 lines.", exception.Message);

			// Cleanup
			File.Delete(inputFilePath);
		}

		// Test for invalid card format
		[Fact]
		public void ReadAndValidate_InvalidCardFormat_ThrowsArgumentException()
		{
			// Arrange
			var lines = new[] { "5 3 S", "6S 7H 8D 9C TH", "INVALID_CARD_FORMAT" };
			File.WriteAllLines(inputFilePath, lines);
			var fileHandler = new FileHandler(inputFilePath);

			// Act & Assert
			var exception = Assert.Throws<ArgumentException>(() => fileHandler.ReadAndValidate());
			Assert.Equal("Invalid card format: INVALID_CARD_FORMAT. Cards must have a valid rank and suit.", exception.Message);

			// Cleanup
			File.Delete(inputFilePath);
		}
	}
}
