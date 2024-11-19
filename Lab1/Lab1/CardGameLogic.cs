using System;

namespace CardGame
{
    public class CardGameLogic
    {
        private readonly string[] playerCards;
        private readonly string[] attackingCards;
        private readonly char trumpSuit;

        public CardGameLogic(string[] playerCards, string[] attackingCards, char trumpSuit)
        {
            this.playerCards = playerCards;
            this.attackingCards = attackingCards;
            this.trumpSuit = trumpSuit;
        }

        // Determines if the player can defend against the attacking cards
        public bool CanDefend()
        {
            foreach (var attackCard in attackingCards)
            {
                bool canDefend = false;

                // Check if there's a higher card of the same suit or a trump card
                foreach (var playerCard in playerCards)
                {
                    if (IsStrongerCard(attackCard, playerCard))
                    {
                        canDefend = true;
                        break;
                    }
                }

                // If no defense was found for this attack card, return false
                if (!canDefend)
                {
                    return false;
                }
            }
            return true; // All attack cards can be defended
        }

        // Determines if a player's card can defend against an attack card
        private bool IsStrongerCard(string attackCard, string playerCard)
        {
            char attackRank = attackCard[0];
            char playerRank = playerCard[0];
            char attackSuit = attackCard[1];
            char playerSuit = playerCard[1];

            // If the attack card is a trump card
            if (attackSuit == trumpSuit)
            {
                return playerSuit == trumpSuit && GetRankValue(playerRank) > GetRankValue(attackRank);
            }
            else
            {
                // For non-trump cards
                return (playerSuit == attackSuit && GetRankValue(playerRank) > GetRankValue(attackRank)) ||
                       (playerSuit == trumpSuit);
            }
        }

        // Gets the value of a rank for comparison
        private int GetRankValue(char rank)
        {
            return rank switch
            {
                '6' => 1,
                '7' => 2,
                '8' => 3,
                '9' => 4,
                'T' => 5,
                'J' => 6,
                'Q' => 7,
                'K' => 8,
                'A' => 9,
                _ => 0,
            };
        }
    }
}
