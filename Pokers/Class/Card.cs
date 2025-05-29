using Poker.Enumerations;
using Poker.Interfaces;

namespace Poker.Classes
{
    public class Card : ICard
    {
        public Suit Suit { get; }
        public CardValue Value { get; }

        public Card(Suit suit, CardValue value)
        {
            Suit = suit;
            Value = value;
        }

        public Suit GetSuit() => Suit;
        public CardValue GetValue() => Value;
        public override string ToString() => $"{Value} of {Suit}";
    }
}