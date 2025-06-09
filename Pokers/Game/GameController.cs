using System;
using Poker.Interfaces;
using Poker.Classes;
using Poker.Enumerations;


namespace Poker.Games
{

    public class GameController
    {
        private readonly List<IPlayer> _players = new();
        private readonly IDeck _deck;
        private readonly ITable _table;
        private readonly IPot _pot;
        private IPlayer? _dealerPlayer;
        private readonly int _smallBlindAmount;
        private readonly int _bigBlindAmount;
        private readonly int _minRaiseAmount;

        public int MinRaise => _minRaiseAmount;
        public int CurrentMaxBet { get; private set; }

        public Action<List<ICard>>? OnCommunityCardsRevealed;
        public Action<IPlayer, ActionType, int>? OnPlayerActionTaken;
        public GameController(int smallBlindAmt, int bigBlindAmt, int minRaiseAmt, IDeck deck, ITable table, IPot pot)
        {
            // Dependensi tidak null
            _deck = deck ?? throw new ArgumentNullException(nameof(deck));
            _table = table ?? throw new ArgumentNullException(nameof(table));
            _pot = pot ?? throw new ArgumentNullException(nameof(pot));

            _smallBlindAmount = smallBlindAmt;
            _bigBlindAmount = bigBlindAmt;
            _minRaiseAmount = minRaiseAmt;
        }

        public void StartNewHand()
        {
            // Hapus pemain yang habis chips
            _players.RemoveAll(p => p.GetChips() <= 0);
            if (_players.Count < 2) return;

            // Reset semua status untuk hand baru
            _deck.ResetAndShuffle(); 
            _table.ClearCommunityCards();
            _pot.SetPot(0);
            CurrentMaxBet = 0;

            foreach (var player in _players)
            {
                player.GetHand().Clear();
                player.SetFolded(false);
                player.SetCurrentBetInRound(0);
            }
            RotateDealerButton();
            AssignBlindsAndPositions();
            DealHoleCards();
            PostBlinds();
        }

        public List<IPlayer> GetSeatedPlayers() => _players.ToList();
        public void SeatPlayer(IPlayer player) => _players.Add(player);
        public void RemovePlayer(IPlayer player) => _players.Remove(player);

        private void RotateDealerButton()
        {
            var activePlayers = _players.Where(p => p.GetChips() > 0).ToList();
            if (!activePlayers.Any()) return;
            int currentIndex = _dealerPlayer != null ? activePlayers.IndexOf(_dealerPlayer) : -1;
            _dealerPlayer = activePlayers[(currentIndex + 1) % activePlayers.Count];
        }

        private void AssignBlindsAndPositions()
        {
            if (_dealerPlayer == null) RotateDealerButton();
            if (_dealerPlayer == null) return;

            var activePlayers = _players.Where(p => p.GetChips() > 0).ToList();
            if (!activePlayers.Any()) return;

            int dealerIndex = activePlayers.IndexOf(_dealerPlayer);
            if (dealerIndex == -1) return;

            activePlayers.ForEach(p => p.SetPosition(Position.None));

            int count = activePlayers.Count;
            if (count == 2) 
            {
                activePlayers[dealerIndex].SetPosition(Position.Dealer); // Dealer juga Small Blind
                activePlayers[(dealerIndex + 1) % count].SetPosition(Position.Big_Blind);
            }
            else
            {
                activePlayers[dealerIndex].SetPosition(Position.Dealer);
                activePlayers[(dealerIndex + 1) % count].SetPosition(Position.Small_Blind);
                activePlayers[(dealerIndex + 2) % count].SetPosition(Position.Big_Blind);

                if (count > 3)
                {
                    activePlayers[(dealerIndex + 3) % count].SetPosition(Position.Early_Position);
                }
                if (count > 4)
                {
                    activePlayers[(dealerIndex + 4) % count].SetPosition(Position.Middle_Position);
                }
                if (count > 5)
                {
                    activePlayers[(dealerIndex + 5) % count].SetPosition(Position.Late_Position);
                }
            }

        }

        private void PostBlinds()
        {
            IPlayer? sbPlayer = _players.FirstOrDefault(p => p.GetPosition() == Position.Small_Blind);
            IPlayer? bbPlayer = _players.FirstOrDefault(p => p.GetPosition() == Position.Big_Blind);

            if (_players.Count(p => p.GetChips() > 0) == 2)
            {
                sbPlayer = _dealerPlayer;
                bbPlayer = _players.First(p => p != _dealerPlayer);
            }

            if (sbPlayer != null)
            {
                int amount = Math.Min(_smallBlindAmount, sbPlayer.GetChips());
                sbPlayer.SetCurrentBetInRound(amount);
                sbPlayer.SetChips(sbPlayer.GetChips() - amount);
                _pot.AddToPot(amount);
            }
            if (bbPlayer != null)
            {
                int amount = Math.Min(_bigBlindAmount, bbPlayer.GetChips());
                bbPlayer.SetCurrentBetInRound(amount);
                bbPlayer.SetChips(bbPlayer.GetChips() - amount);
                _pot.AddToPot(amount);
            }
            CurrentMaxBet = _bigBlindAmount;
        }

        public void StartBettingRound(BettingRoundType round)
        {
            if (round != BettingRoundType.PreFlop)
            {
                foreach (var player in _players.Where(p => !p.IsFolded()))
                {
                    player.SetCurrentBetInRound(0);
                }
                CurrentMaxBet = 0;
            }
        }

        public void HandlePlayerAction(IPlayer player, ActionType action, int amount)
        {
            if (player.IsFolded()) return;

            switch (action)
            {
                case ActionType.Fold:
                    player.SetFolded(true);
                    break;
                case ActionType.Check:
                    break;
                case ActionType.Call:
                    int amountToCall = Math.Min(amount, player.GetChips());
                    player.SetChips(player.GetChips() - amountToCall);
                    player.SetCurrentBetInRound(player.GetCurrentBetInRound() + amountToCall);
                    _pot.AddToPot(amountToCall);
                    break;
                case ActionType.Raise:
                case ActionType.Bet:
                    int amountToRaise = Math.Min(amount, player.GetChips());
                    player.SetChips(player.GetChips() - amountToRaise);
                    player.SetCurrentBetInRound(player.GetCurrentBetInRound() + amountToRaise);
                    _pot.AddToPot(amountToRaise);
                    CurrentMaxBet = player.GetCurrentBetInRound();
                    break;
                case ActionType.AllIn:
                    int allInAmount = player.GetChips();
                    player.SetCurrentBetInRound(player.GetCurrentBetInRound() + allInAmount);
                    player.SetChips(0);
                    _pot.AddToPot(allInAmount);
                    if (player.GetCurrentBetInRound() > CurrentMaxBet)
                    {
                        CurrentMaxBet = player.GetCurrentBetInRound();
                    }
                    break;

            }
            OnPlayerActionTaken?.Invoke(player, action, amount);
        }

        private void DealHoleCards()
        {
            if (_dealerPlayer == null) return;
            int dealerIndex = _players.IndexOf(_dealerPlayer);
            for (int cardNum = 0; cardNum < 2; cardNum++)
            {
                for (int i = 1; i <= _players.Count; i++)
                {
                    var player = _players[(dealerIndex + i) % _players.Count];
                    if (player.GetChips() > 0)
                    {
                        player.AddCardToHand(_deck.DealCard());
                    }
                }
            }
        }

        public void DealCommunityCards(BettingRoundType round)
        {
            _deck.BurnCard();
            if (round == BettingRoundType.Flop)
            {
                for (int i = 0; i < 3; i++) _table.AddCommunityCard(_deck.DealCard());
            }
            else if (round == BettingRoundType.Turn || round == BettingRoundType.River)
            {
                _table.AddCommunityCard(_deck.DealCard());
            }
            OnCommunityCardsRevealed?.Invoke(_table.GetCommunityCards());
        }


        public (HandRank Rank, List<CardValue> HighCards) EvaluateHand(List<ICard> holeCards, List<ICard> communityCards)
        {
            var allCards = new List<ICard>(holeCards);
            allCards.AddRange(communityCards);
            if (allCards.Count < 5) return (HandRank.No_Pair, new List<CardValue>());

            var all5CardHands = GetKCombs(allCards, 5);
            (HandRank Rank, List<CardValue> HighCards) bestHand = (HandRank.No_Pair, new List<CardValue>());

            foreach (var hand in all5CardHands)
            {
                var currentEval = EvaluateSingle5CardHand(hand.ToList());
                if (currentEval.Rank > bestHand.Rank)
                {
                    bestHand = currentEval;
                }
                else if (currentEval.Rank == bestHand.Rank)
                {
                    for (int i = 0; i < currentEval.HighCards.Count; i++)
                    {
                        if (bestHand.HighCards == null || i >= bestHand.HighCards.Count || currentEval.HighCards[i] > bestHand.HighCards[i])
                        {
                            bestHand = currentEval;
                            break;
                        }
                        if (currentEval.HighCards[i] < bestHand.HighCards[i]) break;
                    }
                }
            }
            return bestHand;
        }

        private (HandRank Rank, List<CardValue> HighCards) EvaluateSingle5CardHand(List<ICard> hand)
        {
            var sortedRanks = hand.Select(c => c.Value).OrderByDescending(r => r).ToList();
            var rankGroups = sortedRanks.GroupBy(r => r).OrderByDescending(g => g.Count()).ThenByDescending(g => g.Key).ToList();
            bool isFlush = hand.GroupBy(c => c.Suit).Count() == 1;
            var uniqueRanks = sortedRanks.Distinct().ToList();
            bool isWheel = uniqueRanks.SequenceEqual(new List<CardValue> { CardValue.Ace, CardValue.Five, CardValue.Four, CardValue.Three, CardValue.Two });
            bool isStraight = isWheel || (uniqueRanks.Count == 5 && (int)uniqueRanks.First() - (int)uniqueRanks.Last() == 4);

            var highCards = rankGroups.Select(g => g.Key).ToList();

            if (isStraight && isFlush) return (uniqueRanks.First() == CardValue.Ace ? HandRank.Royal_Flush : HandRank.Straight_Flush, isWheel ? new List<CardValue> { CardValue.Five, CardValue.Four, CardValue.Three, CardValue.Two, CardValue.Ace } : sortedRanks);
            if (rankGroups[0].Count() == 4) return (HandRank.Four_of_a_Kind, new List<CardValue> { rankGroups[0].Key, highCards.First(r => r != rankGroups[0].Key) });
            if (rankGroups[0].Count() == 3 && rankGroups[1].Count() == 2) return (HandRank.Full_House, new List<CardValue> { rankGroups[0].Key, rankGroups[1].Key });
            if (isFlush) return (HandRank.Flush, sortedRanks);
            if (isStraight) return (HandRank.Straight, isWheel ? new List<CardValue> { CardValue.Five, CardValue.Four, CardValue.Three, CardValue.Two, CardValue.Ace } : sortedRanks);
            if (rankGroups[0].Count() == 3) return (HandRank.Three_of_a_Kind, new List<CardValue> { rankGroups[0].Key }.Concat(highCards.Where(r => r != rankGroups[0].Key).Take(2)).ToList());
            if (rankGroups[0].Count() == 2 && rankGroups[1].Count() == 2) return (HandRank.Two_Pairs, new List<CardValue> { rankGroups[0].Key, rankGroups[1].Key, highCards.First(r => r != rankGroups[0].Key && r != rankGroups[1].Key) });
            if (rankGroups[0].Count() == 2) return (HandRank.One_Pair, new List<CardValue> { rankGroups[0].Key }.Concat(highCards.Where(r => r != rankGroups[0].Key).Take(3)).ToList());
            return (HandRank.No_Pair, sortedRanks);
        }

        private static IEnumerable<IEnumerable<T>> GetKCombs<T>(IEnumerable<T> list, int length) where T : ICard
        {
            if (length == 1) return list.Select(t => new T[] { t });
            return GetKCombs(list.Skip(1), length - 1).SelectMany(t => list.Take(1), (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        public void AwardPot(List<IPlayer> winners)
        {
            if (!winners.Any() || _pot.GetAmount() == 0) return;
            int totalPot = _pot.GetAmount();
            int amountPerWinner = totalPot / winners.Count;
            int remainder = totalPot % winners.Count;

            foreach (var winner in winners)
            {
                // Menambahkan bagian pot ke chip pemenang
                winner.SetChips(winner.GetChips() + amountPerWinner); 
            }
    
            // Memberikan sisa jika pembagian tidak rata
            if(remainder > 0 && winners.Any()) 
            {
                winners.First().SetChips(winners.First().GetChips() + remainder);
            }

            _pot.SetPot(0); // Mengosongkan pot setelah dibagikan
        }
    }
}