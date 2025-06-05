using Poker.Enumerations;

namespace Poker.Interfaces
{
    public interface ICard
    {
        Suit Suit { get; }
        CardValue Value { get; }
        HandRank Rank { get;} 

        // Methods
        HandRank GetRank();
        Suit GetSuit();
        CardValue GetValue();
        string ToString();
    }
}