using Poker.Model;
using Poker.Enumerations;
using System.Collections.Generic;

namespace Poker.Interfaces
{
    public interface IDisplay
    {
        void ShowWelcomeMessage();
        void ShowNewHandStarting();
        void ShowRoundBanner(string roundName);
        void DisplayPlayers(List<IPlayer> players, IPot pot);
        void DisplayCommunityCards(ITable table);
        void DisplayPlayerTurn(IPlayer player);
        bool AskToPlayAnotherHand();
        void ShowWinner(IPlayer winner, HandRank rank, int potAmount);
        void ShowSplitPot(List<IPlayer> winners, HandRank rank, int potAmount);
        void ShowFoldWinner(IPlayer winner, int potAmount);
        void ShowShowdown();
        void ShowPlayerHandEvaluation(IPlayer player, HandEvaluationResult evalResult);
        void ShowMessage(string message, ConsoleColor color = ConsoleColor.White);
    }
}