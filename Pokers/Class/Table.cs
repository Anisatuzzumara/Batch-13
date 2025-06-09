using System.Collections.Generic;
using Poker.Interfaces;

namespace Poker.Classes
{
    // Define the ITable interface if it does not exist
    public class Table : ITable
    {
        public List<ICard> communityCards = new List<ICard>();
        public void AddCommunityCard(ICard card) => communityCards.Add(card);

        public void SetCommunityCard(ICard icard)
        {
            if (communityCards.Count < 5 && icard != null) communityCards.Add(icard);
        }

        public List<ICard> GetCommunityCards() => communityCards;
        public void ClearCommunityCards() => communityCards.Clear();
    }
}