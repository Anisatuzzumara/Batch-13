using System;
using System.Collections.Generic;
using Poker.Enumerations;
using Poker.Interfaces;

namespace Poker.Classes
{
    public class Deck : IDeck
    {
        public List<ICard> cards;
        public Random random = new Random();

        public Deck()
        {
            cards = new List<ICard>();
            ResetAndShuffle();
        }

        public void ResetAndShuffle()
        {
            cards.Clear();
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (CardValue value in Enum.GetValues(typeof(CardValue)))
                {
                    cards.Add(new Card(suit, value));
                }
            }
            // Fisher-Yates shuffle
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                ICard value = cards[k];
                cards[k] = cards[n];
                cards[n] = value;
            }
        }

        public ICard DealCard()
        {
            if (cards.Count == 0) throw new InvalidOperationException("No cards left in the deck.");
            var card = cards[0];
            cards.RemoveAt(0);
            return card;
        }
        public void BurnCard()
        {
            if (cards.Count > 0) cards.RemoveAt(0);
        }

        public List<ICard> GetCards() => cards;

        public void SetCards(List<ICard> cards) => this.cards = cards;

        public bool IsEmpty() => cards.Count == 0;
    }
}