using System.Collections.Generic;
using Poker.Enumerations;

namespace Poker.Interfaces
{
    public interface IPlayer
    {
        string GetName();
        int GetChips();
        void SetChips(int chips);

        Position GetPosition();
        void SetPosition(Position position);
        bool IsFolded();
        void SetFolded(bool isFolded);
        int GetCurrentBetInRound();
        void SetCurrentBetInRound(int currentBetInRound);
        List<ICard> GetHand();
        void AddCardToHand(ICard card);

    }
}
