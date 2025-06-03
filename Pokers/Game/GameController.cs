using System;
using System.Collections.Generic;
using System.Linq;
using Poker.Interfaces;
using Poker.Classes;
using Poker.Enumerations;

namespace Poker.Games
{
    public class GameController
    {
        private List<IPlayer> _players = new();
        private IDeck _deck;
        private ITable _table;
        private readonly IPot _pot;
        private IPlayer? _dealerPlayer;
        private int _smallBlindAmount;
        private int _bigBlindAmount;
        private int _minRaise;
        private int _currentBet;
        private int _currentRoundNumber;

        public Action<List<ICard>>? OnCommunityCardsRevealed;
        public Action<IPlayer, ActionType, int>? OnPlayerActionTaken;
        public List<(Player player, ActionType action, int amount)> CurrentRoundActions { get; private set; } = new();

        public GameController(int smallBlindAmt, int bigBlindAmt, int minRaise, IDeck deck, ITable table, IPot pot)
        {
            _smallBlindAmount = smallBlindAmt;
            _bigBlindAmount = bigBlindAmt;
            _minRaise = minRaise;
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
            for (int i = 0; i < _players.Count; i++)
            {
                var position = (i - dealerIndex + _players.Count) % _players.Count;
                if (position == 1)
                    _players[i].SetPosition(Position.Small_Blind);
                else if (position == 2)
                    _players[i].SetPosition(Position.Big_Blind);
                else
                    _players[i].SetPosition(Position.Middle_Position); // fallback
            }
        }

        public void PostBlinds()
        {
            foreach (var player in _players)
            {
                var pos = player.GetPosition();
                if (pos == Position.Small_Blind)
                    player.SetChips(player.GetChips() - _smallBlindAmount);
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
            else if (action == ActionType.Call || action == ActionType.Raise || action == ActionType.Bet)
            {
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
            // Placeholder evaluasi kombinasi kartu
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

        public void StartNewRound()
        {
            StartBettingRound((BettingRoundType)_currentRoundNumber);
            DealCommunityCards((BettingRoundType)_currentRoundNumber);
        }

        public void ManageRoundActions(PlayerAction action)
        {
            if (action.Player != null)
            {
                HandlePlayerAction(action.Player, action.Action, action.Amount);
            }
            // Optionally, handle the case where action.Player is null (e.g., log or throw an exception)
        }
    }

    public class PlayerAction
    {
        public Player? Player { get; set; }
        public ActionType Action { get; set; }
        public int Amount { get; set; }
    }
}


