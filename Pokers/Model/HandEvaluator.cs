using Poker.Enumerations;
using Poker.Interfaces;

namespace Poker.Model
{
    public class HandEvaluator : IHandEvaluator
    {
        public HandEvaluationResult EvaluateHand(List<ICard> holeCards, List<ICard> communityCards)
        {
            var allCards = new List<ICard>(holeCards);
            allCards.AddRange(communityCards);
            if (allCards.Count < 5)
            {
                return new HandEvaluationResult(HandRank.NoPair, new List<CardValue>());
            }

            var all5CardHands = GetKCombs(allCards, 5);
            HandEvaluationResult? bestHand = null;

            foreach (var hand in all5CardHands)
            {
                var currentEval = EvaluateSingle5CardHand(hand.ToList());

                if (bestHand == null || currentEval.Rank > bestHand.Rank)
                {
                    bestHand = currentEval;
                }
                else if (currentEval.Rank == bestHand.Rank)
                {
                    for (int i = 0; i < currentEval.HighCards.Count; i++)
                    {
                        if (i >= bestHand.HighCards.Count || currentEval.HighCards[i] > bestHand.HighCards[i])
                        {
                            bestHand = currentEval;
                            break;
                        }
                        if (currentEval.HighCards[i] < bestHand.HighCards[i])
                            break;
                    }
                }
            }
            return bestHand ?? new HandEvaluationResult(HandRank.NoPair, new List<CardValue>());
        }

        public HandEvaluationResult EvaluateSingle5CardHand(List<ICard> hand)
        {
            HandEvaluationResult result; ;

            var sortedRanks = hand.Select(c => c.Value).OrderByDescending(r => r).ToList();
            var rankGroups = sortedRanks.GroupBy(r => r).OrderByDescending(g => g.Count()).ThenByDescending(g => g.Key).ToList();
            bool isFlush = hand.GroupBy(c => c.Suit).Count() == 1;
            var uniqueRanks = sortedRanks.Distinct().ToList();
            bool isWheel = uniqueRanks.SequenceEqual(new List<CardValue> { CardValue.Ace, CardValue.Five, CardValue.Four, CardValue.Three, CardValue.Two });
            bool isStraight = isWheel || (uniqueRanks.Count == 5 && (int)uniqueRanks.First() - (int)uniqueRanks.Last() == 4);
            var highCards = rankGroups.Select(g => g.Key).ToList();

            if (isStraight && isFlush)
            {
                var rank = uniqueRanks.First() == CardValue.Ace ? HandRank.RoyalFlush : HandRank.StraightFlush;
                var cards = isWheel ? new List<CardValue> { CardValue.Five, CardValue.Four, CardValue.Three, CardValue.Two, CardValue.Ace } : sortedRanks;
                result = new HandEvaluationResult(rank, cards);
            }
            else if (rankGroups[0].Count() == 4)
            {
                var cards = new List<CardValue> { rankGroups[0].Key, highCards.First(r => r != rankGroups[0].Key) };
                result = new HandEvaluationResult(HandRank.FourOfAKind, cards);
            }
            else if (rankGroups.Count > 1 && rankGroups[0].Count() == 3 && rankGroups[1].Count() >= 2)
            {
                var cards = new List<CardValue> { rankGroups[0].Key, rankGroups[1].Key };
                result = new HandEvaluationResult(HandRank.FullHouse, cards);
            }
            else if (isFlush)
            {
                result = new HandEvaluationResult(HandRank.Flush, sortedRanks);

            }
            else if (isStraight)
            {
                var cards = isWheel ? new List<CardValue>
                { CardValue.Five, CardValue.Four, CardValue.Three, CardValue.Two, CardValue.Ace } : sortedRanks;
                result = new HandEvaluationResult(HandRank.Straight, cards);
            }
            else if (rankGroups[0].Count() == 3)
            {
                var cards = new List<CardValue>
                { rankGroups[0].Key }.Concat(highCards.Where(r => r != rankGroups[0].Key).Take(2)).ToList();
                result = new HandEvaluationResult(HandRank.ThreeOfAKind, cards);
            }
            else if (rankGroups.Count > 1 && rankGroups[0].Count() == 2 && rankGroups[1].Count() == 2)
            {
                var cards = new List<CardValue>
                { rankGroups[0].Key, rankGroups[1].Key, highCards.First(r => r != rankGroups[0].Key && r != rankGroups[1].Key) };
                result = new HandEvaluationResult(HandRank.TwoPairs, cards);
            }
            else
            {
                var cards = new List<CardValue>
                { rankGroups[0].Key }.Concat(highCards.Where(r => r != rankGroups[0].Key).Take(3)).ToList();
                result = new HandEvaluationResult(HandRank.OnePair, cards);
            }

            return result;
        }

        public static IEnumerable<IEnumerable<T>> GetKCombs<T>(IEnumerable<T> list, int length) where T : ICard
        {
            IEnumerable<IEnumerable<T>> result;
            if (length == 0)
            {
                return new[] { Enumerable.Empty<T>() };
            }
            else if (!list.Any())
            {
                result = Enumerable.Empty<IEnumerable<T>>();
            }
            else
            {
                var firstElement = list.First();
                var restOfList = list.Skip(1);

                var combsWithoutFirst = GetKCombs(restOfList, length);
                var combsWithFirst = GetKCombs(restOfList, length - 1).Select(comb => new[] { firstElement }.Concat(comb));

                result = combsWithFirst.Concat(combsWithoutFirst);
            }

            return result;
        }
    }
}