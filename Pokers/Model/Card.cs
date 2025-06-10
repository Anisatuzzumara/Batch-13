using Poker.Enumerations;
using Poker.Interfaces;

namespace Poker.Model
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

        public Suit GetSuit()
        {
            return Suit;
        }

        public CardValue GetValue()
        {
            return Value;
        }

        public override string ToString()
        {
            return $"{Value} of {Suit}";
        }
    }
}