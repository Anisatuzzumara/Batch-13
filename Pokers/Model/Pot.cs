using Poker.Interfaces;

namespace Poker.Model
{

    public class Pot : IPot
    {
        public int Amount { get; set; }
        public Pot(int amount)
        {
            Amount = amount;
        }

        public void SetPot(int amount)
        {
            Amount = amount;
        }

        public int GetAmount()
        {
            return Amount;
        }

        public void AddToPot(int amount)
        {
            Amount += amount;
        }
    }
}