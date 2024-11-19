using System;
using Xunit;

namespace CardGame.Tests
{
    public class CardGameLogicTests
    {
        // Test for successful defense with trump cards
        [Fact]
        public void CanDefend_WithTrumpCards_ReturnsTrue()
        {
            // Arrange
            var playerCards = new[] { "6H", "9H", "TH", "JC", "AS" }; // Player has trump cards
            var attackingCards = new[] { "7S", "8D", "TH" }; // Attacking cards
            char trumpSuit = 'H';

            var gameLogic = new CardGameLogic(playerCards, attackingCards, trumpSuit);

            // Act
            bool canDefend = gameLogic.CanDefend();

            // Assert
            Assert.True(canDefend);
        }

        // Test for successful defense with higher cards of the same suit
        [Fact]
        public void CanDefend_WithHigherCards_ReturnsTrue()
        {
            // Arrange
            var playerCards = new[] { "9S", "QS", "KH", "AC" }; // Player has cards of the same suit
            var attackingCards = new[] { "8S", "9C", "JS" }; // Attacking cards
            char trumpSuit = 'H';

            var gameLogic = new CardGameLogic(playerCards, attackingCards, trumpSuit);

            // Act
            bool canDefend = gameLogic.CanDefend();

            // Assert
            Assert.True(canDefend);
        }

        // Test for failure to defend
        [Fact]
        public void CanDefend_NoDefendingCards_ReturnsFalse()
        {
            // Arrange
            var playerCards = new[] { "6D", "7C", "8C" }; // Player has no cards to defend
            var attackingCards = new[] { "9S", "TS", "JS" }; // Attacking cards
            char trumpSuit = 'H';

            var gameLogic = new CardGameLogic(playerCards, attackingCards, trumpSuit);

            // Act
            bool canDefend = gameLogic.CanDefend();

            // Assert
            Assert.False(canDefend);
        }
    }
}
