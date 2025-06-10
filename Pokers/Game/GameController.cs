using System;
using System.Collections.Generic;
using System.Linq;
using Poker.Interfaces;
using Poker.Model;
using Poker.Enumerations;

namespace Poker.Games
{
    public class GameController
    {
        private List<IPlayer> _players = new();
        private IDeck _deck;
        private ITable _table;
        private IPot _pot;
        private IHandEvaluator _handEvaluator;
        private IPlayer? _dealerPlayer;
        private int _smallBlindAmount;
        private int _bigBlindAmount;
        public int minRaiseAmount;
        public int CurrentMaxBet { get; set; }

        public Action<IPlayer>? onDealerButtonRotated;
        public Action<List<ICard>>? onCommunityCardsRevealed;
        public Action<IPlayer, ActionType, int>? onPlayerActionTaken;
        
        public GameController(int smallBlindAmt, int bigBlindAmt, int minRaiseAmt, IDeck deck, ITable table, IPot pot, IHandEvaluator handEvaluator)
        {
            _deck = deck;
            _table = table;
            _pot = pot;
            _handEvaluator = handEvaluator;
            _smallBlindAmount = smallBlindAmt;
            _bigBlindAmount = bigBlindAmt;
            minRaiseAmount = minRaiseAmt;
        }

        public void StartGame()
        {
            StartNewHand();
        }

        public void StartNewHand()
        {
            _players.RemoveAll(p => p.GetChips() <= 0);
            if (_players.Count < 2)
                return;
            ResetDeck();
            ShuffleDeck();
            ClearCommunityCards();
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
        
        
        public void ResetDeck()
        {
            var newCards = new List<ICard>();
            
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (CardValue rank in Enum.GetValues(typeof(CardValue)))
                {
                    newCards.Add(new Card(suit, rank));
                }
            }
            _deck.SetCards(newCards);
        }
        
        public void ShuffleDeck()
        {
            var cards = _deck.GetCards();
            var random = new Random();
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                ICard value = cards[k];
                cards[k] = cards[n];
                cards[n] = value;
            }
        }
        
        public void ClearCommunityCards()
        {
            _table.GetCommunityCards().Clear();
        }
        public ICard DealCard()
        {
            var cards = _deck.GetCards();
            var card = cards[0];
            cards.RemoveAt(0);
            return card;
        }

        public void BurnCard()
        {
            var cards = _deck.GetCards();
            if (cards.Count > 0)
            {
                cards.RemoveAt(0);
            }
        }
        
        public HandEvaluationResult EvaluateHand(List<ICard> holeCards, List<ICard> communityCards)
        {
            return _handEvaluator.EvaluateHand(holeCards, communityCards);
        }
        
        public List<IPlayer> GetSeatedPlayers() => _players.ToList();
        public void SeatPlayer(IPlayer player) => _players.Add(player);
        public void RemovePlayer(IPlayer player) => _players.Remove(player);

        public void RotateDealerButton()
        {
            var activePlayers = _players.Where(p => p.GetChips() > 0).ToList();
            if (!activePlayers.Any())
                return;
            int currentIndex = _dealerPlayer != null ? activePlayers.IndexOf(_dealerPlayer) : -1;
            _dealerPlayer = activePlayers[(currentIndex + 1) % activePlayers.Count];
            onDealerButtonRotated?.Invoke(_dealerPlayer);
        }
        public void AssignBlindsAndPositions()
        {
            if (_dealerPlayer == null)
                RotateDealerButton();
            if (_dealerPlayer == null)
                return;
            var activePlayers = _players.Where(p => p.GetChips() > 0).ToList();
            if (!activePlayers.Any())
                return;
            int dealerIndex = activePlayers.IndexOf(_dealerPlayer);
            if (dealerIndex == -1)
                return;
            activePlayers.ForEach(p => p.SetPosition(Position.None));
            int count = activePlayers.Count;
            if (count == 2)
            {
                activePlayers[dealerIndex].SetPosition(Position.Dealer);
                activePlayers[(dealerIndex + 1) % count].SetPosition(Position.BigBlind);
            }
            else
            {
                activePlayers[dealerIndex].SetPosition(Position.Dealer);
                activePlayers[(dealerIndex + 1) % count].SetPosition(Position.SmallBlind);
                activePlayers[(dealerIndex + 2) % count].SetPosition(Position.BigBlind);
                if (count > 3) activePlayers[(dealerIndex + 3) % count].SetPosition(Position.EarlyPosition);
                if (count > 4) activePlayers[(dealerIndex + 4) % count].SetPosition(Position.MiddlePosition);
                else if (count > 5) activePlayers[(dealerIndex + 5) % count].SetPosition(Position.LatePosition);
            }
        }
        private void PostBlinds()
        {
            IPlayer? sbPlayer = _players.FirstOrDefault(p => p.GetPosition() == Position.SmallBlind);
            IPlayer? bbPlayer = _players.FirstOrDefault(p => p.GetPosition() == Position.BigBlind);
            if (_players.Count(p => p.GetChips() > 0) == 2)
            {
                sbPlayer = _dealerPlayer;
                bbPlayer = _players.First(p => p != _dealerPlayer);
            }
            if (sbPlayer != null)
            {
                int amount = Math.Min(_smallBlindAmount, sbPlayer.GetChips());
                HandlePlayerAction(sbPlayer, ActionType.Bet, amount);
            }
            if (bbPlayer != null)
            {
                int amount = Math.Min(_bigBlindAmount, bbPlayer.GetChips());
                if (sbPlayer != null) CurrentMaxBet = sbPlayer.GetCurrentBetInRound();
                else
                {
                    CurrentMaxBet = 0;
                }
                HandlePlayerAction(bbPlayer, ActionType.Bet, amount);
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
            if (player.IsFolded())
                return;
            switch (action)
            {
                case ActionType.Fold: player.SetFolded(true);
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
                    if (player.GetCurrentBetInRound() > CurrentMaxBet) CurrentMaxBet = player.GetCurrentBetInRound();
                    break;
            }
            onPlayerActionTaken?.Invoke(player, action, amount);
        }
        public void DealHoleCards()
        {
            if (_dealerPlayer == null)
                return;
            int dealerIndex = _players.IndexOf(_dealerPlayer);
            for (int cardNum = 0; cardNum < 2; cardNum++)
            {
                for (int i = 1; i <= _players.Count; i++)
                {
                    var player = _players[(dealerIndex + i) % _players.Count];
                    if (player.GetChips() > 0) player.AddCardToHand(DealCard());
                }
            }
        }
        public void DealCommunityCards(BettingRoundType round)
        {
            BurnCard();
            if (round == BettingRoundType.Flop)
                for (int i = 0; i < 3; i++) _table.AddCommunityCard(DealCard());
            if (round == BettingRoundType.Turn || round == BettingRoundType.River)
            {
                _table.AddCommunityCard(DealCard());
            }
            onCommunityCardsRevealed?.Invoke(_table.GetCommunityCards());
        }
        public void AwardPot(List<IPlayer> winners)
        {
            if (!winners.Any() || _pot.GetAmount() == 0)
                return;
            int totalPot = _pot.GetAmount();
            int amountPerWinner = totalPot / winners.Count;
            int remainder = totalPot % winners.Count;
            foreach (var winner in winners) winner.SetChips(winner.GetChips() + amountPerWinner);
            if (remainder > 0 && winners.Any()) winners.First().SetChips(winners.First().GetChips() + remainder);
            _pot.SetPot(0);
        }
        
    }
}