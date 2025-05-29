using Poker.Enumerations;

namespace Poker.Interfaces
{
    public interface ICard
    {
        Suit Suit { get; }
        CardValue Value { get; }
        string ToString();
    }
}