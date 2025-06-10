using Poker.Enumerations;
using System.Collections.Generic;

namespace Poker.Model
{
    public class HandEvaluationResult
    {
        public HandRank Rank { get; }
        public List<CardValue> HighCards { get; }

        public HandEvaluationResult(HandRank rank, List<CardValue> highCards)
        {
            Rank = rank;
            HighCards = highCards ?? new List<CardValue>();
        }
    }
}