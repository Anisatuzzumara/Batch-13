namespace Poker.Interfaces
{
    public interface IPot
    {
        void SetPot(int amount);
        int GetAmount();
        void AddToPot(int amount);
        
    }
}