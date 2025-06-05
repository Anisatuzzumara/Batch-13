using System.Collections.Generic;      

namespace Poker.Interfaces
{
    public interface ITable
    {
        void SetCommunityCard(ICard card);
        List<ICard> GetCommunityCards();
    }
}