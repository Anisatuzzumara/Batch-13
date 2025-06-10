using System;
using System.Collections.Generic;
using Poker.Enumerations;
using Poker.Interfaces;

namespace Poker.Model
{
    public class Deck : IDeck
    {
        public List<ICard> cards = new List<ICard>();
        public Random random = new Random();

        public List<ICard> GetCards()
        {
            return cards;
        }

        public void SetCards(List<ICard> cards)
        {
            this.cards = cards;
        }

        public bool IsEmpty()
        {
            return cards.Count == 0;
        }
    }
}