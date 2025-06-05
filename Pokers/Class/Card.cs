using Poker.Enumerations;
using Poker.Interfaces;

namespace Poker.Classes
{
    public class Card : ICard
    {
        public Suit Suit { get; }
        public CardValue Value { get; }
        public HandRank Rank { get; }

        public Card(Suit suit, CardValue value, HandRank rank)
        {
            Suit = suit;
            Value = value;
            Rank = rank;
        }

        public HandRank GetRank() => Rank;
        public Suit GetSuit() => Suit;
        public CardValue GetValue() => Value;
        public override string ToString() => $"{Value} of {Suit}";
    }
}