using Poker.Model; 
using System.Collections.Generic;

namespace Poker.Interfaces
{
    public interface IHandEvaluator
    {
        HandEvaluationResult EvaluateHand(List<ICard> holeCards, List<ICard> communityCards);
    }
}