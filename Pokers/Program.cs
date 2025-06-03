using Poker.Classes;
using Poker.Enumerations;
using Poker.Games;
using Poker.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Poker
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Inisialisasi komponen permainan
            IDeck deck = new Deck();
            ITable table = new Table();
            IPot pot = new Pot(0);
            GameController game = new GameController(10, 20, 20, deck, table, pot);

            // Tambahkan pemain
            Console.Write("Masukkan jumlah pemain (2-4): ");
            int numPlayers = int.Parse(Console.ReadLine() ?? "2");

            for (int i = 1; i <= numPlayers; i++)
            {
                Console.Write($"Nama pemain {i}: ");
                string name = Console.ReadLine() ?? $"Pemain{i}";
                game.SeatPlayer(new Player(name, 500));
            }

            Console.Clear();
            game.StartGame();
            game.PostBlinds();

            Console.WriteLine("=== Pre-Flop ===");
            DisplayHands(game);
            DisplayPot(pot);

            RunBettingRound(game, pot);

            Console.Write("\n=== Flop ===");
            game.StartNewRound(); // Flop
            DisplayCommunityCards(table);
            RunBettingRound(game, pot);

            Console.Write("\n=== Turn ===");
            game.StartNewRound(); // Turn
            DisplayCommunityCards(table);
            RunBettingRound(game, pot);

            Console.Write("\n=== River ===");
            game.StartNewRound(); // River
            DisplayCommunityCards(table);
            RunBettingRound(game, pot);

            Console.Write("\n=== Showdown ===");
            DisplayHands(game);
            var winners = game.FindWinners(game.GetSeatedPlayers(), table.GetCommunityCards());
            Console.WriteLine("Pemenang:");
            foreach (var winner in winners)
                Console.WriteLine($"🏆 {winner.GetName()} memenangkan pot!");

            game.AwardPot(winners.Cast<Poker.Interfaces.IPlayer>().ToList());
            DisplayPot(pot);
        }

        static void RunBettingRound(GameController game, IPot pot)
        {
            foreach (var player in game.GetSeatedPlayers())
            {
                if (player.IsFolded()) continue;

                Console.WriteLine($"\n{player.GetName()} (Chips: {player.GetChips()}) - Giliran Anda!");
                Console.Write("Aksi (fold, call, raise): ");
                string action = Console.ReadLine()?.ToLower() ?? "call";
                ActionType type = ActionType.Call;
                int amount = 0;

                switch (action)
                {
                    case "fold":
                        type = ActionType.Fold;
                        break;
                    case "raise":
                        type = ActionType.Raise;
                        Console.Write("Jumlah raise: ");
                        amount = int.Parse(Console.ReadLine() ?? "20");
                        break;
                    default:
                        type = ActionType.Call;
                        amount = 20;
                        break;
                }

                game.HandlePlayerAction(player, type, amount);
            }

            DisplayPot(pot);
        }

        static void DisplayHands(GameController game)
        {
            foreach (var player in game.GetSeatedPlayers())
            {
                Console.WriteLine($"\n{player.GetName()} {(player.IsFolded() ? "[Folded]" : "")}:");
                PrintHandWithVisual(player.GetHand());
            }
        }

        static void DisplayCommunityCards(ITable table)
        {
            Console.WriteLine("Kartu di meja:");
            PrintHandWithVisual(table.GetCommunityCards());
        }

        static void DisplayPot(IPot pot)
        {
            Console.WriteLine($"\nTotal Pot: {pot.GetAmount()} chips\n");
        }

        static void PrintHandWithVisual(List<Poker.Interfaces.ICard> cards)
        {
            foreach (var card in cards)
            {
                PrintCard(card);
            }
            Console.WriteLine();
        }

        static void PrintCard(Poker.Interfaces.ICard card)
        {
            string symbol = card.Suit switch
            {
                Suit.Hearts => "♥",
                Suit.Spades => "♠",
                Suit.Diamonds => "♦",
                Suit.Clubs => "♣",
                _ => "?"
            };

            ConsoleColor original = Console.ForegroundColor;
            if (card.Suit == Suit.Hearts || card.Suit == Suit.Diamonds)
                Console.ForegroundColor = ConsoleColor.Red;
            else
                Console.ForegroundColor = ConsoleColor.White;

            string val = card.Value switch
            {
                CardValue.Ace => "A",
                CardValue.King => "K",
                CardValue.Queen => "Q",
                CardValue.Jack => "J",
                _ => ((int)card.Value).ToString()
            };

            string cardString = $"┌────┐\n" +
                                $"│{val,-2}  │\n" +
                                $"│ {symbol}  │\n" +
                                $"│  {val,2}│\n" +
                                $"└────┘";

            Console.WriteLine(cardString);
            Console.ForegroundColor = original;
        }
    }
}
