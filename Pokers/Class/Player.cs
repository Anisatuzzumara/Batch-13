using System.Collections.Generic;
using Poker.Enumerations;
using Poker.Interfaces;

namespace Poker.Classes
{
    public class Player : IPlayer
    {
        public string Name { get; }
        public int Chips { get; private set; }
        public Position Positions { get; set; }
        public bool Folded { get; set; }
        public int CurrentBetInRound  { get; set; }
        public List<ICard> Hand { get; }
        public Player(string name, int chips)
        {
            Name = name;
            Chips = chips;
            Hand = new List<ICard>();
            Folded = false;
            CurrentBetInRound = 0;
            
        }

        public string GetName() => Name;
        public int GetChips() => Chips;
        public void SetChips(int chips) => Chips = chips;
        public Position GetPosition() => Positions;
        public void SetPosition(Position position) => Positions = position;
        public bool IsFolded() => Folded;
        public void SetFolded(bool folded) => Folded = folded;
        public int GetCurrentBetInRound() => CurrentBetInRound;
        public void SetCurrentBetInRound(int bet) => CurrentBetInRound = bet;
        public List<ICard> GetHand() => Hand;
    }
}