using Poker.Interfaces;

namespace Poker.Classes
{

    public class Pot : IPot
    {
        public int Amount { get; set; }
        public Pot(int amount)
        {
            Amount = amount;
        }

        public void SetPot(int amount) => Amount = amount;
        public int GetAmount() => Amount;

    }
}