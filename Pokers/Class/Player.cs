using System.Collections.Generic;
using Poker.Enumerations;
using Poker.Interfaces;

namespace Poker.Classes
{
    public class Player : IPlayer
    {
        public string Name { get; }
        public int Chips { get; set; }
        public Position CurrentPositions { get; set; }
        public bool FoldedStatus { get; set; }
        public int CurrentBetInRound { get; set; }
        public List<ICard> Hand { get; }
        public bool HasActedThisTurn { get; set; }
        public HandRank FinalHandRank { get; set; }
        public List<CardValue> FinalHighCards { get; set; } = new List<CardValue>();
        public int BestHandValue { get; set; }

        public Player(string name, int chips)
        {
            Name = name;
            Chips = chips;
            Hand = new List<ICard>();
        }

        public string GetName() => Name;
        public int GetChips() => Chips;
        public void SetChips(int chips) => Chips = chips;
        public Position GetPosition() => CurrentPositions;
        public void SetPosition(Position position) => CurrentPositions = position;
        public bool IsFolded() => FoldedStatus;
        public void SetFolded(bool folded) => FoldedStatus = folded;
        public int GetCurrentBetInRound() => CurrentBetInRound;
        public void SetCurrentBetInRound(int bet) => CurrentBetInRound = bet;
        public List<ICard> GetHand() => Hand;
        public void AddCardToHand(ICard card) => Hand.Add(card);

        
        
    }
}