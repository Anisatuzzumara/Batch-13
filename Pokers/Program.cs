﻿using Poker.Classes;
using Poker.Enumerations;
using Poker.Games;
using Poker.Interfaces;
using System.Text;

class Program
{
    static ITable table = new Table();
    static IPot pot;
    static GameController controller = null!;

    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Title = "Texas Hold'em Poker";
        Console.WriteLine("=== Selamat Datang di Permainan Poker Texas Hold'em ===");

        pot = new Pot(0);
        int playerCount = GetPlayerCount(2, 6);
        IDeck deck = new Deck();
        controller = new GameController(10, 20, 20, deck, table, pot);
        controller.OnPlayerActionTaken += (player, action, amount) => { Console.WriteLine($"-> Aksi: {player.GetName()} melakukan {action} sebesar {amount}"); };

        for (int i = 1; i <= playerCount; i++)
        {
            Console.Write($"Masukkan nama Pemain {i}: ");
            string name = Console.ReadLine() ?? $"Pemain{i}";
            if (string.IsNullOrWhiteSpace(name)) name = $"Pemain{i}";
            int chips = GetPlayerChips(i, 1000);
            controller.SeatPlayer(new Player(name, chips));
        }

        // Versi OTOMATIS
        //for (int i = 1; i <= playerCount; i++)
        //{
        //    Console.Write($"Masukkan nama Pemain {i}: ");
        //    string name = Console.ReadLine() ?? $"Pemain{i}";
        //    if (string.IsNullOrWhiteSpace(name)) name = $"Pemain{i}";
        //    int chips = 1000; // <-- UBAH DI SINI. Semua pemain akan mulai dengan 1000 chip.
        //    controller.SeatPlayer(new Player(name, chips));
        //}

        bool continuePlaying = true;
        while (continuePlaying)
        {
            if (controller.GetSeatedPlayers().Count(p => p.GetChips() > 0) < 2)
            {
                Console.WriteLine("\nTidak cukup pemain untuk melanjutkan. Permainan selesai.");
                break;
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n--- Memulai Hand Baru ---");
            Console.ResetColor();
            
            controller.StartNewHand();
            
            PrintPlayers();
            RunGameRound(BettingRoundType.PreFlop, "Pre-Flop");
            RunGameRound(BettingRoundType.Flop, "Flop");
            RunGameRound(BettingRoundType.Turn, "Turn");
            RunGameRound(BettingRoundType.River, "River");

            PerformShowdown();

            Console.Write("\nMainkan hand lain? (Y/N): ");
            continuePlaying = (Console.ReadLine()?.Trim().ToUpper() == "Y");
        }
        Console.WriteLine("\nTerima kasih telah bermain!");
    }

    static void RunGameRound(BettingRoundType roundType, string roundName)
    {
        if (controller.GetSeatedPlayers().Count(p => !p.IsFolded()) <= 1) return;

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n--- Babak {roundName} ---");
        Console.ResetColor();

        if (roundType != BettingRoundType.PreFlop)
        {
            controller.DealCommunityCards(roundType);
        }
        
        PrintCommunityCards();
        HandleBettingRound(roundType);
    }
    
    static void HandleBettingRound(BettingRoundType roundType)
    {
        controller.StartBettingRound(roundType);
        
        var playersInOrder = GetPlayersInBettingOrder(roundType);
        if (!playersInOrder.Any()) return;

        int playerIndex = 0;
        int actionCount = 0;
        
        while(true)
        {
            var activePlayers = controller.GetSeatedPlayers().Where(p => !p.IsFolded()).ToList();
            if (activePlayers.Count <= 1) break;

            IPlayer currentPlayer = playersInOrder[playerIndex % playersInOrder.Count];

            if (!currentPlayer.IsFolded() && currentPlayer.GetChips() > 0)
            {
                RequestAndHandlePlayerAction(currentPlayer);
                actionCount++;
            }
            
            playerIndex++;

            // Cek kondisi akhir babak
            var playersStillIn = controller.GetSeatedPlayers().Where(p => !p.IsFolded()).ToList();
            var maxBet = controller.CurrentMaxBet;
            bool allBetsMatched = playersStillIn.All(p => p.GetCurrentBetInRound() == maxBet || p.GetChips() == 0);

            // Babak berakhir jika semua orang sudah bertindak dan taruhan sudah cocok.
            // Pengecekan actionCount >= playersStillIn.Count memastikan semua orang mendapat giliran setelah raise terakhir.
            if(allBetsMatched && actionCount >= playersStillIn.Count)
            {
                break;
            }
            
            // Safety break untuk menghindari loop tak terbatas
            if (playerIndex > playersInOrder.Count * 4) break; 
        }
    }

    static void RequestAndHandlePlayerAction(IPlayer player)
    {
        Console.WriteLine($"\n> Giliran {player.GetName()} (Chips: {player.GetChips()})");
        PrintHand(player.GetHand());

        int amountToCall = controller.CurrentMaxBet - player.GetCurrentBetInRound();
        bool canCheck = amountToCall <= 0;

        Console.Write("  Opsi: (F) Fold");
        if(canCheck) Console.Write(" | (K) Check");
        else Console.Write($" | (C) Call [{Math.Min(amountToCall, player.GetChips())}]");
        if(player.GetChips() > amountToCall) Console.Write(" | (R) Raise");
        Console.WriteLine($" | (A) All-in");

        while (true)
        {
            Console.Write("  Aksi Anda: ");
            string? input = Console.ReadLine()?.Trim().ToUpper();
            switch (input)
            {
                case "F": controller.HandlePlayerAction(player, ActionType.Fold, 0); return;
                case "K":
                    if (!canCheck) { Console.WriteLine("  Tidak bisa Check, harus Call."); continue; }
                    controller.HandlePlayerAction(player, ActionType.Check, 0);
                    return;
                case "C":
                    if (canCheck) { Console.WriteLine("  Tidak perlu Call, cukup Check."); continue; }
                    controller.HandlePlayerAction(player, ActionType.Call, amountToCall);
                    return;
                case "R":
                    if(player.GetChips() <= amountToCall) { Console.WriteLine("  Chip tidak cukup untuk Raise."); continue; }
                    Console.Write($"  Masukkan jumlah TAMBAHAN untuk Raise (min {controller.MinRaise}): ");
                    if (int.TryParse(Console.ReadLine(), out int raiseAmount) && raiseAmount >= controller.MinRaise)
                    {
                        int totalCommitment = amountToCall + raiseAmount;
                        if(totalCommitment >= player.GetChips()) { Console.WriteLine("  Jumlah tidak valid. Gunakan All-in."); continue; }
                        controller.HandlePlayerAction(player, ActionType.Raise, totalCommitment);
                        return;
                    }
                    Console.WriteLine("  Jumlah tidak valid.");
                    continue;
                case "A":
                    controller.HandlePlayerAction(player, ActionType.AllIn, 0); // Amount diabaikan, controller akan menggunakan semua chip
                    return;
                default:
                    Console.WriteLine("  Input salah.");
                    break;
            }
        }
    }

    static List<IPlayer> GetPlayersInBettingOrder(BettingRoundType roundType)
    {
        var activePlayers = controller.GetSeatedPlayers();
        int dealerIndex = activePlayers.FindIndex(p => p.GetPosition() == Position.Dealer);
        if (dealerIndex == -1) dealerIndex = 0;

        int startIndex;
        if (roundType == BettingRoundType.PreFlop)
        {
            if (activePlayers.Count == 2) startIndex = dealerIndex; // Heads-up, Dealer/SB bertindak dulu
            else startIndex = (dealerIndex + 3) % activePlayers.Count; 
        }
        else
        {
             if (activePlayers.Count == 2) startIndex = (dealerIndex + 1) % 2; // Heads-up, BB bertindak dulu
             else startIndex = (dealerIndex + 1) % activePlayers.Count; // SB
        }

        List<IPlayer> orderedList = new();
        for (int i = 0; i < activePlayers.Count; i++)
        {
            orderedList.Add(activePlayers[(startIndex + i) % activePlayers.Count]);
        }
        return orderedList;
    }

    static void PerformShowdown()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n--- SHOWDOWN ---");
        Console.ResetColor();

        var activePlayers = controller.GetSeatedPlayers().Where(p => !p.IsFolded()).ToList();
        int potAmount = pot.GetAmount();

        if (activePlayers.Count <= 1)
        {
            if (activePlayers.Any())
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n🏆 Pemenang: {activePlayers.First().GetName()} karena semua pemain lain telah Fold. memenangkan pot sebesar {potAmount}!");
                Console.ResetColor();
                controller.AwardPot(activePlayers);
            }
            return;
        }

        PrintCommunityCards();

        Dictionary<IPlayer, (HandRank Rank, List<CardValue> HighCards)> evaluations = new();
        foreach (var player in activePlayers.Cast<Player>())
        {
            var eval = controller.EvaluateHand(player.GetHand(), table.GetCommunityCards());
            evaluations.Add(player, eval);
            player.FinalHandRank = eval.Rank;
            player.FinalHighCards = eval.HighCards;
            Console.WriteLine($"\n{player.GetName()} memiliki: {GetReadableHandRank(eval.Rank)} ({string.Join(", ", eval.HighCards.Select(RankToString))})");
            PrintHand(player.GetHand());
        }

        var bestRank = evaluations.Values.Max(e => e.Rank);
        var potentialWinners = evaluations.Where(kvp => kvp.Value.Rank == bestRank).ToList();
        
        List<IPlayer> finalWinners = potentialWinners.OrderByDescending(p => p.Value.HighCards, new ListComparer<CardValue>()).Select(p => p.Key).ToList();
        var bestHandHighCards = finalWinners.Cast<Player>().First().FinalHighCards;
        finalWinners = finalWinners.Where(p => ((Player)p).FinalHighCards.SequenceEqual(bestHandHighCards)).ToList();
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n🏆 Pemenang: {string.Join(" & ", finalWinners.Select(p => p.GetName()))} dengan {GetReadableHandRank(bestRank)} memenangkan pot sebesar {potAmount}!");
        Console.ResetColor();
        controller.AwardPot(finalWinners);
    }
      

    static void PrintPlayers()
    {
        Console.WriteLine("\n--- Status Pemain ---");
        foreach (var p in controller.GetSeatedPlayers())
        {
            string status = p.IsFolded() ? " (Folded)" : (p.GetChips() == 0 ? " (All-In)" : "");           
            string pos = p.GetPosition() switch {
                Position.Dealer => "(D)",
                Position.Small_Blind => "(SB)",
                Position.Big_Blind => "(BB)",
                Position.Early_Position => "(EP)",
                Position.Middle_Position => "(MP)",
                Position.Late_Position => "(LP)"
            };
            // ---------------------------------

            Console.WriteLine($"{p.GetName(), -10}{pos,-5}| Chips: {p.GetChips(),-5} | Bet: {p.GetCurrentBetInRound(),-4}{status}");
        }
        Console.WriteLine($"Total Pot: {pot.GetAmount()}");
    }

    static void PrintCommunityCards()
    {
        Console.WriteLine("\n--- Kartu Meja ---");
        var community = table.GetCommunityCards();
        if (!community.Any()) Console.WriteLine("(Belum ada kartu)");
        else PrintHand(community);
    }

    static void PrintHand(List<ICard> cards)
    {
        if (cards == null || !cards.Any()) return;
        List<string[]> cardVisuals = cards.Select(GetCardVisual).ToList();
        for (int i = 0; i < 5; i++) Console.WriteLine(string.Join(" ", cardVisuals.Select(c => c[i])));
        
    }

    static string[] GetCardVisual(ICard card)
    {
        string rank = card.Value switch { CardValue.Ten => "10", CardValue.Jack => "J", CardValue.Queen => "Q", CardValue.King => "K", CardValue.Ace => "A", _ => ((int)card.Value).ToString() };
        string suit = card.Suit switch { Suit.Spades => "♠", Suit.Hearts => "♥", Suit.Diamonds => "♦", Suit.Clubs => "♣", _ => "?" };
        return new[] { "┌───────┐", $"| {rank.PadRight(2)}    |", $"|   {suit}   |", $"|    {rank.PadLeft(2)} |", "└───────┘" };
    }

    static int GetPlayerCount(int min, int max)
    {
        while (true)
        {
            Console.Write($"Masukkan jumlah pemain ({min}-{max}): ");
            if (int.TryParse(Console.ReadLine(), out int count) && count >= min && count <= max) return count;
            Console.WriteLine("Input tidak valid.");
        }
    }

    static int GetPlayerChips(int playerNumber, int defaultChips)
    {
        while (true)
        {
            Console.Write($"Masukkan chip untuk Pemain {playerNumber} (default: {defaultChips}): ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) return defaultChips;
            if (int.TryParse(input, out int chips) && chips > 20) return chips;
            Console.WriteLine("Input tidak valid. Chip harus lebih besar dari Big Blind (20).");
        }
    }

    static string GetReadableHandRank(HandRank r) => r.ToString().Replace("_", " ");
    static string RankToString(CardValue r) => r switch {
        CardValue.Ten => "T", CardValue.Jack => "J", CardValue.Queen => "Q", CardValue.King => "K", CardValue.Ace => "A", _ => ((int)r).ToString()
    };
}

public class ListComparer<T> : IComparer<List<T>> where T : IComparable
{
    public int Compare(List<T>? x, List<T>? y)
    {
        if (x == null || y == null) return 0;
        for (int i = 0; i < Math.Min(x.Count, y.Count); i++)
        {
            int c = x[i].CompareTo(y[i]);
            if (c != 0) return c;
        }
        return x.Count.CompareTo(y.Count);
    }
}