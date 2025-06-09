using System.Collections.Generic;


namespace Poker.Interfaces
{
    public interface IDeck
    {
        List<ICard> GetCards();
        void SetCards(List<ICard> cards);
        bool IsEmpty();
        void ResetAndShuffle();
        ICard DealCard();
        void BurnCard();
    }
}