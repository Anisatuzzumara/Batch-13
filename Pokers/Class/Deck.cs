using System;
using System.Collections.Generic;
using Poker.Enumerations;
using Poker.Interfaces;

namespace Poker.Classes
{
    public class Deck : IDeck
    {
        public List<ICard> cards;
        public Random rnd = new Random();

        public Deck()
        {
            cards = new List<ICard>();
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
                foreach (CardValue value in Enum.GetValues(typeof(CardValue)))
                    cards.Add(new Card(suit, value));

        }

        public List<ICard> GetCards() => cards;

        public void SetCards(List<ICard> cards) => this.cards = cards;

        public bool IsEmpty() => cards.Count == 0;

        public void Shuffle()
        {
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                var value = cards[k];
                cards[k] = cards[n];
                cards[n] = value;
            }
        }

        public ICard Deal()
        {
            if (cards.Count == 0) throw new InvalidOperationException("No cards left in the deck.");
            var card = cards[0];
            cards.RemoveAt(0);
            return card;
        }
    }
}