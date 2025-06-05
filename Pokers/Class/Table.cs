using System.Collections.Generic;
using Poker.Interfaces;

namespace Poker.Classes
{
    // Define the ITable interface if it does not exist
    public class Table : ITable
    {
        public List<ICard> communityCards = new List<ICard>();

        public void SetCommunityCard(ICard icard) => communityCards.Add(icard);
        public List<ICard> GetCommunityCards() => communityCards;
    }
}