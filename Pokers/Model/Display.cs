using Poker.Model;
using Poker.Enumerations;
using Poker.Interfaces;
using System.Text;

namespace Poker.Model
{

    public class ConsoleDisplay : IDisplay
    {
        public void ShowWelcomeMessage()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "Texas Hold'em Poker";
            Console.WriteLine("=== Selamat Datang di Permainan Poker Texas Hold'em ===");
        }

        public void ShowNewHandStarting()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n--- Memulai Hand Baru ---");
            Console.ResetColor();
        }

        public void ShowRoundBanner(string roundName)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n--- Babak {roundName} ---");
            Console.ResetColor();
        }

        public void DisplayPlayers(List<IPlayer> players, IPot pot)
        {
            Console.WriteLine("\n--- Status Pemain ---");
            foreach (var p in players)
            {
                string status = p.IsFolded() ? " (Folded)" : (p.GetChips() == 0 ? " (All-In)" : "");
                string pos = p.GetPosition() switch
                {
                    Position.Dealer => "(D)",
                    Position.SmallBlind => "(SB)",
                    Position.BigBlind => "(BB)",
                    Position.EarlyPosition => "(EP)",
                    Position.MiddlePosition => "(MP)",
                    Position.LatePosition => "(LP)",
                    _ => ""
                };
                Console.WriteLine($"{p.GetName(),-10}{pos,-5}| Chips: {p.GetChips(),-5} | Bet: {p.GetCurrentBetInRound(),-4}{status}");
            }
            Console.WriteLine($"Total Pot: {pot.GetAmount()}");
        }

        public void DisplayCommunityCards(ITable table)
        {
            Console.WriteLine("\n--- Kartu Meja ---");
            var community = table.GetCommunityCards();
            if (!community.Any()) Console.WriteLine("(Belum ada kartu)");
            else DisplayHand(community);
        }

        public void DisplayPlayerTurn(IPlayer player)
        {
            Console.WriteLine($"\n> Giliran {player.GetName()} (Chips: {player.GetChips()}, Bet: {player.GetCurrentBetInRound()})");
            DisplayHand(player.GetHand());
        }

        public bool AskToPlayAnotherHand()
        {
            Console.Write("\nMainkan hand lain? (Y/N): ");
            return (Console.ReadLine()?.Trim().ToUpper() == "Y");
        }

        public void ShowWinner(IPlayer winner, HandRank rank, int potAmount)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n🏆 Pemenang: {winner.GetName()} dengan {GetReadableHandRank(rank)}, memenangkan pot sebesar {potAmount}!");
            Console.ResetColor();
        }

        public void ShowSplitPot(List<IPlayer> winners, HandRank rank, int potAmount)
        {
            int amountPerWinner = potAmount / winners.Count;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n🤝 Pot Dibagi! {string.Join(" & ", winners.Select(p => p.GetName()))} dengan {GetReadableHandRank(rank)}, masing-masing memenangkan {amountPerWinner}.");
            Console.ResetColor();
        }

        public void ShowFoldWinner(IPlayer winner, int potAmount)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n🏆 Pemenang: {winner.GetName()} karena semua pemain lain telah Fold, memenangkan pot sebesar {potAmount}!");
            Console.ResetColor();
        }

        public void ShowShowdown()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n--- SHOWDOWN ---");
            Console.ResetColor();
        }

        public void ShowPlayerHandEvaluation(IPlayer player, HandEvaluationResult evalResult)
        {
            Console.WriteLine($"\n{player.GetName()} memiliki: {GetReadableHandRank(evalResult.Rank)} ({string.Join("-", evalResult.HighCards.Select(RankToString))})");
            DisplayHand(player.GetHand());
        }

        public void ShowMessage(string message, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void DisplayHand(List<ICard> cards)
        {
            if (cards == null || !cards.Any()) return;
            List<string[]> cardVisuals = cards.Select(GetCardVisual).ToList();
            if (!cardVisuals.Any() || cardVisuals[0] == null) return;

            for (int i = 0; i < 5; i++) // Asumsi tinggi kartu adalah 5 baris
            {
                for (int j = 0; j < cards.Count; j++)
                {
                    ICard currentCard = cards[j];
                    string cardLine = cardVisuals[j][i];
                    if (currentCard.Suit == Suit.Hearts || currentCard.Suit == Suit.Diamonds) { Console.BackgroundColor = ConsoleColor.DarkRed; Console.ForegroundColor = ConsoleColor.White; }
                    else { Console.BackgroundColor = ConsoleColor.Black; Console.ForegroundColor = ConsoleColor.White; }
                    Console.Write(cardLine);
                    Console.ResetColor();
                    Console.Write("  ");
                }
                Console.WriteLine();
            }
        }

        public string[] GetCardVisual(ICard card)
        {
            string rank = card.Value switch { CardValue.Ten => "10", CardValue.Jack => "J", CardValue.Queen => "Q", CardValue.King => "K", CardValue.Ace => "A", _ => ((int)card.Value).ToString() };
            string suit = card.Suit switch { Suit.Spades => "♠", Suit.Hearts => "♥", Suit.Diamonds => "♦", Suit.Clubs => "♣", _ => "?" };
            if (rank == "10") return new[] { "┌───────┐", $"| {rank}    |", $"|   {suit}   |", $"|    {rank} |", "└───────┘" };
            return new[] { "┌───────┐", $"| {rank.PadRight(2)}    |", $"|   {suit}   |", $"|    {rank.PadLeft(2)} |", "└───────┘" };
        }

        public string GetReadableHandRank(HandRank r) => r.ToString().Replace("_", " ");
        public string RankToString(CardValue r) => r switch
        {
            CardValue.Ten => "T",
            CardValue.Jack => "J",
            CardValue.Queen => "Q",
            CardValue.King => "K",
            CardValue.Ace => "A",
            _ => ((int)r).ToString()
        };
    }
}