namespace Poker.Games
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Poker.Interfaces;
    using Poker.Classes;
    using Poker.Enumerations;

    public class GameController
    {
        private List<IPlayer> _players = new();
        private IDeck _deck;
        private ITable _table;
        private IPot _pot;
        private IPlayer? _dealerPlayer;
        private int _smallBlindAmount;
        private int _bigBlindAmount;
        private int _minRaise;
        private int _currentBet;
        private int _currentRoundNumber;

        public Action<List<ICard>>? OnCommunityCardsRevealed;
        public Action<IPlayer, ActionType, int>? OnPlayerActionTaken;
        public List<(Player player, ActionType action, int amount)> CurrentRoundActions { get; private set; } = new();
        public int BigBlind { get; internal set; }
        public int MinRaise => _minRaise; 

        public int CurrentMaxBet { get;  set; }

        public GameController(int smallBlindAmt, int bigBlindAmt, int minRaise, IDeck deck, ITable table, IPot pot)
        {
            _smallBlindAmount = 100;
            _bigBlindAmount = 1000;
            _minRaise = 100;
            _deck = deck;
            _table = table;
            _pot = pot;
        }


        public void StartGame()
        {
            ShuffleCard();
            StartNewHand();
        }

        public List<IPlayer> GetSeatedPlayers() => _players;

        public void ShuffleCard()
        {
            var cards = _deck.GetCards();
            var random = new Random();
            cards = cards.OrderBy(_ => random.Next()).ToList();
            _deck.SetCards(cards);
        }

        public void StartNewHand()
        {
            _table.GetCommunityCards().Clear();
            foreach (var player in _players)
            {
                player.GetHand().Clear();
                player.SetFolded(false);
                player.SetCurrentBetInRound(0);
            }
            RotateDealerButton();
            AssignBlindsAndPositions();
            DealHoleCards();
        }


        public void SeatPlayer(IPlayer player) => _players.Add(player);
        public void RemovePlayer(IPlayer player) => _players.Remove(player);

        public void RotateDealerButton()
        {
            if (_players.Count == 0) return;
            var currentIndex = _dealerPlayer != null ? _players.IndexOf(_dealerPlayer) : -1;
            _dealerPlayer = _players[(currentIndex + 1) % _players.Count];
        }

        public void AssignBlindsAndPositions()
        {
            if (_dealerPlayer == null)
            return;
            var dealerIndex = _players.IndexOf(_dealerPlayer);
            int count = _players.Count;
            for (int i = 0; i < count; i++)
            {
            var position = (i - dealerIndex + count) % count;
            if (position == 0)
                _players[i].SetPosition(Position.Dealer);
            else if (position == 1)
                _players[i].SetPosition(Position.Small_Blind);
            else if (position == 2)
                _players[i].SetPosition(Position.Big_Blind);
            else if (position == 3)
                _players[i].SetPosition(Position.Early_Position);
            else if (position == count - 1)
                _players[i].SetPosition(Position.Middle_Position);
            else
                _players[i].SetPosition(Position.Late_Position);
            }
        }


        public void PostBlinds()
        {
            foreach (var player in _players)
            {
                var pos = player.GetPosition();
                if (pos == Position.Small_Blind)
                    player.SetChips(player.GetChips() - _smallBlindAmount);
                if (pos == Position.Dealer)
                    player.SetChips(player.GetChips() - _smallBlindAmount); // or use a specific dealer blind amount if needed
                else if (pos == Position.Big_Blind)
                    player.SetChips(player.GetChips() - _bigBlindAmount);
            }
        }

        public List<ICard> TakeAllCards(IPlayer player)
        {
            var cards = player.GetHand();
            player.GetHand().Clear();
            return cards;
        }


        public void AddPlayerChips(IPlayer player, int amount) => player.SetChips(player.GetChips() + amount);

        public void ProcessFold(IPlayer player) => player.SetFolded(true);

        public void ClearCommunityCards() => _table.GetCommunityCards().Clear();

        public List<ICard> ShowPlayerHand(IPlayer player) => player.GetHand();

        public void StartBettingRound(BettingRoundType round)
        {
            _currentRoundNumber++;
            _currentBet = 0;
            CurrentRoundActions.Clear();
            // Tambahkan logika betting round di sini jika perlu
        }

        public PlayerAction RequestPlayerAction(int currentBet, int minRaise, int potSize, List<ICard> communityCards)
        {
            // Placeholder: implementasi sesuai kebutuhan UI/CLI
            return new PlayerAction();
        }

        public void HandlePlayerAction(IPlayer player, ActionType action, int amount)
        {
            if (action == ActionType.Fold)
                ProcessFold(player);
            else if (action == ActionType.Call || action == ActionType.Raise || action == ActionType.Bet || action == ActionType.AllIn || action == ActionType.Check)
            {
                // Validation is handled below.
            }
            else
            {
                throw new InvalidOperationException($"Unknown action type: {action}");
            }
            {
                if (player.IsFolded())
                    throw new InvalidOperationException("Cannot take action on a folded player.");

                if (amount < 0)
                    throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be non-negative.");

                if (amount > player.GetChips())
                    throw new InvalidOperationException("Player does not have enough chips to perform this action.");

                if (action == ActionType.Call && amount < _currentBet)
                    throw new InvalidOperationException($"Call amount must be at least the current bet: {_currentBet}");
            }
            {
                // Enforce minimum raise
                if (action == ActionType.Raise && amount < _minRaise)
                    throw new InvalidOperationException($"Raise amount must be at least the minimum raise: {_minRaise}");

                player.SetChips(player.GetChips() - amount);
                player.SetCurrentBetInRound(player.GetCurrentBetInRound() + amount);
                _pot.SetPot(_pot.GetAmount() + amount);
                if (amount > _currentBet)
                    _currentBet = amount;
            }
            CurrentRoundActions.Add(((Player)player, action, amount));
            OnPlayerActionTaken?.Invoke(player, action, amount);
        }

        public void DealHoleCards()
        {
            for (int i = 0; i < 2; i++)
            {
                foreach (var player in _players)
                {
                    var card = DealCard();
                    player.GetHand().Add(card);
                }
            }
        }


        public void DealCommunityCards(BettingRoundType round)
        {
            if (round == BettingRoundType.Flop)
            {
                BurnCard();
                _table.SetCommunityCard(DealCard());
                _table.SetCommunityCard(DealCard());
                _table.SetCommunityCard(DealCard());
            }
            else if (round == BettingRoundType.Turn || round == BettingRoundType.River)
            {
                BurnCard();
                _table.SetCommunityCard(DealCard());
            }

            OnCommunityCardsRevealed?.Invoke(_table.GetCommunityCards());
        }


        public ICard DealCard()
        {
            var card = _deck.GetCards().First();
            _deck.GetCards().RemoveAt(0);
            return card;
        }

        public void BurnCard() => _deck.GetCards().RemoveAt(0);

        public HandRank EvaluateHand(List<ICard> holeCards, List<ICard> communityCards)
        {
            var allCards = holeCards.Concat(communityCards).ToList();

            // Hitung jumlah rank dan suit
            var rankGroups = allCards.GroupBy(c => c.Rank).OrderByDescending(g => g.Count()).ToList();
            var suitGroups = allCards.GroupBy(c => c.Suit).ToList();

            bool isFlush = suitGroups.Any(g => g.Count() >= 5);
            var orderedRanks = allCards.Select(c => (int)c.Rank).Distinct().OrderByDescending(r => r).ToList();

            // Cek Straight
            bool isStraight = false;
            int straightHigh = 0;
            for (int i = 0; i <= orderedRanks.Count - 5; i++)
            {
            if (orderedRanks[i] - orderedRanks[i + 4] == 4)
            {
                isStraight = true;
                straightHigh = orderedRanks[i];
                break;
            }
            }
            // Cek straight low-Ace (A,2,3,4,5)
            if (!isStraight && orderedRanks.Contains(14) && orderedRanks.Contains(2) && orderedRanks.Contains(3) && orderedRanks.Contains(4) && orderedRanks.Contains(5))
            {
            isStraight = true;
            straightHigh = 5;
            }

            // Cek Royal Flush & Straight Flush
            if (isFlush)
            {
            foreach (var suitGroup in suitGroups.Where(g => g.Count() >= 5))
            {
                var flushRanks = suitGroup.Select(c => (int)c.Rank).Distinct().OrderByDescending(r => r).ToList();
                // Cek straight flush
                for (int i = 0; i <= flushRanks.Count - 5; i++)
                {
                if (flushRanks[i] - flushRanks[i + 4] == 4)
                {
                    if (flushRanks[i] == 14)
                    return HandRank.Royal_Flush;
                    return HandRank.Straight_Flush;
                }
                }
                // Cek straight flush low-Ace
                if (flushRanks.Contains(14) && flushRanks.Contains(2) && flushRanks.Contains(3) && flushRanks.Contains(4) && flushRanks.Contains(5))
                return HandRank.Straight_Flush;
            }
            }

            // Four of a Kind
            if (rankGroups.Any(g => g.Count() == 4))
            return HandRank.Four_of_a_Kind;

            // Full House
            if (rankGroups.Any(g => g.Count() == 3) && rankGroups.Any(g => g.Count() == 2))
            return HandRank.Full_House;

            // Flush
            if (isFlush)
            return HandRank.Flush;

            // Straight
            if (isStraight)
            return HandRank.Straight;

            // Three of a Kind
            if (rankGroups.Any(g => g.Count() == 3))
            return HandRank.Three_of_a_Kind;

            // Two Pair
            if (rankGroups.Count(g => g.Count() == 2) >= 2)
            return HandRank.Two_Pairs;

            // One Pair
            if (rankGroups.Any(g => g.Count() == 2))
            return HandRank.One_Pair;

            // High Card
            return HandRank.No_Pair;
        }

        public List<Player> FindWinners(List<IPlayer> players, List<ICard> communityCards)
        {
            var remainingPlayers = players.Where(p => !p.IsFolded()).Cast<Player>();
            var ranked = remainingPlayers
                .Select(p => new { Player = p, Rank = EvaluateHand(p.GetHand(), communityCards) })
                .OrderByDescending(x => x.Rank)
                .ToList();

            var bestRank = ranked.First().Rank;
            return ranked.Where(x => x.Rank == bestRank).Select(x => x.Player).ToList();
        }


        public void AwardPot(List<IPlayer> winners)
        {
            int splitAmount = _pot.GetAmount() / winners.Count;
            foreach (var winner in winners)
                winner.SetChips(winner.GetChips() + splitAmount);
            _pot.SetPot(0);
        }

        public void ResetGameState()
        {
            _currentRoundNumber = 0;
            _currentBet = 0;
            CurrentRoundActions.Clear();
            ClearCommunityCards();
        }


        public List<ICard> GetAllCards(List<ICard> communityCards)
        {
            var all = new List<ICard>();
            foreach (var p in _players)
            all.AddRange(p.GetHand());
            all.AddRange(communityCards);
            return all;
        }

        // (Assume this is near the end of your GameController class)
        // Returns a numeric value representing the strength of a hand for comparison
        public int GetHandValue(List<ICard> playerHand, List<ICard> communityCards)
        {
            // Example: Combine all cards and assign a simple value based on sorted ranks
            var allCards = playerHand.Concat(communityCards).OrderByDescending(c => (int)c.Rank).ToList();
            int value = 0;
            int multiplier = 1;
            foreach (var card in allCards)
            {
                value += (int)card.Rank * multiplier;
                multiplier *= 15; // 13 ranks + buffer
            }
        return value;
        }
    

        public void StartNewRound()
        {
            StartBettingRound((BettingRoundType)_currentRoundNumber);
            DealCommunityCards((BettingRoundType)_currentRoundNumber);
        }

        public void ManageRoundActions(PlayerAction action)
        {
            HandlePlayerAction(action.Player, action.Action, action.Amount);
        }
    }

    public class PlayerAction
    {
        public Player? Player { get; set; }
        public ActionType Action { get; set; }
        public int Amount { get; set; }
    }
}