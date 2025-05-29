// See https://aka.ms/new-console-template for more information
// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using Poker.Interfaces;
using Poker.Classes;
using Poker.Enumerations;
using Poker.Games;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        IDeck deck = new Deck();
        ITable table = new Table();
        IPot pot = new Pot(0);
        var game = new GameController(10, 20, 20, deck, table, pot);

        Console.WriteLine("Masukkan jumlah pemain (2-4):");
        int playerCount = int.Parse(Console.ReadLine() ?? "2");
        for (int i = 0; i < playerCount; i++)
        {
            Console.Write($"Masukkan nama pemain {i + 1}: ");
            string name = Console.ReadLine() ?? $"Player{i + 1}";

            Console.Write($"Masukkan jumlah chips untuk {name}: ");
            int chips = int.Parse(Console.ReadLine() ?? "1000");

            IPlayer player = new Player(name, chips);
            game.SeatPlayer(player);
        }

        game.StartGame();
        game.PostBlinds();

        foreach (var player in game.GetSeatedPlayers())
        {
            Console.WriteLine($"\nKartu untuk {player.GetName()}: (Chips: {player.GetChips()})");
            PrintHandWithComplexBorder(player.GetHand());
        }

        // Flop
        game.DealCommunityCards(BettingRoundType.Flop);
        Console.WriteLine("\nFlop:");
        PrintHandWithComplexBorder(table.GetCommunityCards());

        // Turn
        game.DealCommunityCards(BettingRoundType.Turn);
        Console.WriteLine("\nTurn:");
        PrintHandWithComplexBorder(table.GetCommunityCards());

        // River
        game.DealCommunityCards(BettingRoundType.River);
        Console.WriteLine("\nRiver:");
        PrintHandWithComplexBorder(table.GetCommunityCards());

        // Aksi pemain
        foreach (var player in game.GetSeatedPlayers())
        {
            Console.WriteLine($"\nGiliran {player.GetName()} (Chips: {player.GetChips()}):");
            Console.WriteLine("Pilih aksi: 1. Fold  2. Call  3. Raise");
            string? actionInput = Console.ReadLine();
            ActionType action = actionInput switch
            {
                "1" => ActionType.Fold,
                "2" => ActionType.Call,
                "3" => ActionType.Raise,
                _ => ActionType.Check
            };
            int amount = 0;
            if (action == ActionType.Call || action == ActionType.Raise)
            {
                Console.Write("Masukkan jumlah taruhan: ");
                amount = int.Parse(Console.ReadLine() ?? "0");
            }

            game.HandlePlayerAction(player, action, amount);
        }

        var winners = game.FindWinners(game.GetSeatedPlayers(), table.GetCommunityCards());
        game.AwardPot(winners.ConvertAll<IPlayer>(w => w));

        Console.WriteLine("\nPemenang:");
        foreach (var winner in winners)
            Console.WriteLine($"{winner.GetName()} memenangkan pot! (Total chips: {winner.GetChips()})");
    }

    static void PrintHandWithComplexBorder(IEnumerable<ICard> hand)
    {
        var cards = new List<ICard>(hand);

        foreach (var card in cards)
            Console.Write("┌───────┐ ");
        Console.WriteLine();

        foreach (var card in cards)
        {
            string valueStr = card.Value.ToString();
            if (valueStr.Length > 2) valueStr = valueStr[..2];
            SetCardColor(card);
            Console.Write($"│{valueStr,-2}     │");
            Console.ResetColor();
            Console.Write(" ");
        }
        Console.WriteLine();

        foreach (var card in cards)
        {
            SetCardColor(card);
            Console.Write("│       │");
            Console.ResetColor();
            Console.Write(" ");
        }
        Console.WriteLine();

        foreach (var card in cards)
        {
            string symbol = card.GetSuit() switch
            {
                Suit.Hearts => "♥",
                Suit.Diamonds => "♦",
                Suit.Clubs => "♣",
                Suit.Spades => "♠",
                _ => "?"
            };
            SetCardColor(card);
            Console.Write($"│   {symbol}   │");
            Console.ResetColor();
            Console.Write(" ");
        }
        Console.WriteLine();

        foreach (var card in cards)
        {
            SetCardColor(card);
            Console.Write("│       │");
            Console.ResetColor();
            Console.Write(" ");
        }
        Console.WriteLine();

        foreach (var card in cards)
        {
            string valueStr = card.Value.ToString();
            if (valueStr.Length > 2) valueStr = valueStr[..2];
            SetCardColor(card);
            Console.Write($"│     {valueStr,2}│");
            Console.ResetColor();
            Console.Write(" ");
        }
        Console.WriteLine();

        foreach (var card in cards)
            Console.Write("└───────┘ ");
        Console.WriteLine();
    }

    static void SetCardColor(ICard card)
    {
        switch (card.GetSuit())
        {
            case Suit.Hearts:
            case Suit.Diamonds:
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Red;
                break;
            case Suit.Clubs:
            case Suit.Spades:
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.ForegroundColor = ConsoleColor.Black;
                break;
        }
    }
}
