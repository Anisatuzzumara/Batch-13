using System.Collections.Generic;
using Poker.Interfaces;

namespace Poker.Model
{
    
    public class Table : ITable
    {
        public List<ICard> communityCards = new List<ICard>();
        public void AddCommunityCard(ICard card)
        {
            communityCards.Add(card);
        }

        public void SetCommunityCard(ICard icard)
        {
            if (communityCards.Count < 5 && icard != null) communityCards.Add(icard);
        }

        public List<ICard> GetCommunityCards()
        {
            return communityCards;
        }
   }
}