using Poker.Model;
using Poker.Enumerations;
using Poker.Games;
using Poker.Interfaces;



class Program
{

    static ITable table = new Table();
    static IPot pot;
    static GameController controller = null!;
    static IDisplay display = new ConsoleDisplay(); 
    static IHandEvaluator handEvaluator = new HandEvaluator();

    static void Main(string[] args)
    {
        display.ShowWelcomeMessage();
        pot = new Pot(0);

        int playerCount = GetPlayerCount(2, 6);
        IDeck deck = new Deck();
        
        controller = new GameController(10, 20, 20, deck, table, pot, handEvaluator);

        // Registrasi pemain
        for (int i = 1; i <= playerCount; i++)
        {
            Console.Write($"Masukkan nama Pemain {i}: ");
            string name = Console.ReadLine() ?? $"Pemain{i}";
            if (string.IsNullOrWhiteSpace(name)) name = $"Pemain{i}";
            {
                int chips = GetPlayerChips(i, 1000);
                controller.SeatPlayer(new Player(name, chips));
            }
        }

        // Loop permainan utama
        bool continuePlaying = true;
        while (continuePlaying)
        {
            if (controller.GetSeatedPlayers().Count(p => p.GetChips() > 0) < 2)
            {
                display.ShowMessage("\nTidak cukup pemain untuk melanjutkan. Permainan selesai.", ConsoleColor.Red);
                break;
            }

            display.ShowNewHandStarting();
            controller.StartNewHand();
            
            RunGameRound(BettingRoundType.PreFlop, "Pre-Flop");
            RunGameRound(BettingRoundType.Flop, "Flop");
            RunGameRound(BettingRoundType.Turn, "Turn");
            RunGameRound(BettingRoundType.River, "River");

            PerformShowdown();
            continuePlaying = display.AskToPlayAnotherHand();
        }
        Console.WriteLine("\nTerima kasih telah bermain!");
    }

    static void RunGameRound(BettingRoundType roundType, string roundName)
    {
        if (controller.GetSeatedPlayers().Count(p => !p.IsFolded()) <= 1)
            return;
        {
            display.ShowRoundBanner(roundName);
            display.DisplayPlayers(controller.GetSeatedPlayers(), pot);
        }
        if (roundType != BettingRoundType.PreFlop)
            {
                controller.DealCommunityCards(roundType);
            }
        
        display.DisplayCommunityCards(table);
        HandleBettingRound(roundType);
    }
        static void HandleBettingRound(BettingRoundType roundType)
    {
        controller.StartBettingRound(roundType);
        
        var playersInOrder = GetPlayersInBettingOrder(roundType);
        if (!playersInOrder.Any())
            return;
        
        int playerIndex = 0;
        int playersActedInTurn = 0;
        
        while(true)
        {
            var activePlayers = controller.GetSeatedPlayers().Where(p => !p.IsFolded()).ToList();
            if (activePlayers.Count <= 1)
                break;

            IPlayer currentPlayer = playersInOrder[playerIndex % playersInOrder.Count];

            bool didRaiseOrBet = false;
            if (!currentPlayer.IsFolded() && currentPlayer.GetChips() > 0)
            {
                didRaiseOrBet = RequestAndHandlePlayerAction(currentPlayer);
                playersActedInTurn++;
            }
            
            if (didRaiseOrBet)
            {
                playersActedInTurn = 1; 
                playersInOrder = GetPlayersInBettingOrder(roundType);
                playerIndex = playersInOrder.IndexOf(currentPlayer);
            }

            var maxBet = controller.CurrentMaxBet;
            bool allBetsMatched = activePlayers.All(p => p.GetCurrentBetInRound() == maxBet || p.GetChips() == 0);

            if (playersActedInTurn >= playersInOrder.Count && allBetsMatched)
                break;
            
            playerIndex++;
            if (playerIndex > playersInOrder.Count * 4)
                break; 
        }
    }

    static bool RequestAndHandlePlayerAction(IPlayer player)
    {
        display.DisplayPlayerTurn(player);

        int amountToCall = controller.CurrentMaxBet - player.GetCurrentBetInRound();
        bool canCheck = amountToCall <= 0;
        bool hasRaisedOrBet = false;

        Console.Write("  Opsi: (F)old");
        if (canCheck) Console.Write(" | (K)heck");
        else
        {
            Console.Write($" | (C)all [{Math.Min(amountToCall, player.GetChips())}]");
        }
        if (player.GetChips() > amountToCall) Console.Write(" | (R)aise");
        Console.WriteLine($" | (A)ll-in");

        while (true)
        {
            Console.Write("  Aksi Anda: ");
            string? input = Console.ReadLine()?.Trim().ToUpper();
            switch (input)
            {
                case "F": controller.HandlePlayerAction(player, ActionType.Fold, 0);
                    break;
                case "K":
                    if (!canCheck)
                    {
                        display.ShowMessage("  Tidak bisa Check, harus Call.", ConsoleColor.Red);
                    }
                    controller.HandlePlayerAction(player, ActionType.Check, 0);
                    break;
                case "C":
                    if (canCheck)
                    {
                        display.ShowMessage("  Tidak perlu Call, cukup Check.", ConsoleColor.Red);
                    }
                    controller.HandlePlayerAction(player, ActionType.Call, amountToCall);
                    break;
                case "R":
                    if (player.GetChips() <= amountToCall)
                    {
                        display.ShowMessage("  Chip tidak cukup untuk Raise.", ConsoleColor.Red);
                    }
                    Console.Write($"  Masukkan jumlah TOTAL taruhan baru (min {controller.CurrentMaxBet + controller.minRaiseAmount}): ");
                    if (int.TryParse(Console.ReadLine(), out int totalBet) && totalBet >= controller.CurrentMaxBet + controller.minRaiseAmount)
                    {
                        int amountToCommit = totalBet - player.GetCurrentBetInRound();
                        if(amountToCommit > player.GetChips()) { display.ShowMessage("  Jumlah melebihi chip Anda.", ConsoleColor.Red); }
                        controller.HandlePlayerAction(player, ActionType.Raise, amountToCommit); 
                        hasRaisedOrBet = true;
                        break;
                    }
                    display.ShowMessage("  Jumlah tidak valid.", ConsoleColor.Red);
                    break;
                case "A":
                    int oldMaxBet = controller.CurrentMaxBet;
                    controller.HandlePlayerAction(player, ActionType.AllIn, player.GetChips()); 
                    if(controller.CurrentMaxBet > oldMaxBet) hasRaisedOrBet = true;
                    break;
                default:
                    display.ShowMessage("  Input salah. Gunakan F, K, C, R, atau A.", ConsoleColor.Red); 
                    break;
            }
            return hasRaisedOrBet;
        }
    }
    
    static List<IPlayer> GetPlayersInBettingOrder(BettingRoundType roundType)
    {
        var activePlayers = controller.GetSeatedPlayers();
        int dealerIndex = activePlayers.FindIndex(p => p.GetPosition() == Position.Dealer);
        if (dealerIndex == -1)
        {
            dealerIndex = 0;
        }
        int startIndex;
        if (roundType == BettingRoundType.PreFlop)
        {
            if (activePlayers.Count == 2) startIndex = dealerIndex;
            else
            {
                startIndex = (dealerIndex + 3) % activePlayers.Count;
            }
        }
        else
        {
            if (activePlayers.Count == 2) startIndex = (dealerIndex + 1) % 2;
            else
            {
                startIndex = (dealerIndex + 1) % activePlayers.Count;
            }
        }
        List<IPlayer> orderedList = new();
        for (int i = 0; i < activePlayers.Count; i++) orderedList.Add(activePlayers[(startIndex + i) % activePlayers.Count]);
        return orderedList.Where(p => !p.IsFolded()).ToList();
    }
    
    static void PerformShowdown()
    {
        display.ShowShowdown();
        var activePlayers = controller.GetSeatedPlayers().Where(p => !p.IsFolded()).ToList();
        int potAmount = pot.GetAmount();
        if (activePlayers.Count <= 1)
        {
            if (activePlayers.Any())
            {
                display.ShowFoldWinner(activePlayers.First(), potAmount);
                controller.AwardPot(activePlayers);
            }
            return;
        }
        display.DisplayCommunityCards(table);
        Dictionary<IPlayer, HandEvaluationResult> evaluations = new();

        foreach (var player in activePlayers.Cast<Player>())
        {
            var eval = controller.EvaluateHand(player.GetHand(), table.GetCommunityCards());
            evaluations.Add(player, eval);
            player.FinalHandRank = eval.Rank;
            player.FinalHighCards = eval.HighCards;
            display.ShowPlayerHandEvaluation(player, eval);
        }

        var bestRank = evaluations.Values.Max(e => e.Rank);
        var potentialWinners = evaluations.Where(kvp => kvp.Value.Rank == bestRank).ToList();
        List<IPlayer> finalWinners;
        if (potentialWinners.Count == 1) finalWinners = new List<IPlayer>
        {
            potentialWinners.First().Key
        };
        else  
        {
            finalWinners = potentialWinners.OrderByDescending(p => p.Value.HighCards, new ListRankComparer()).Select(p => p.Key).ToList();
            var bestKickers = ((Player)finalWinners.First()).FinalHighCards;
            finalWinners = finalWinners.Where(p => ((Player)p).FinalHighCards.SequenceEqual(bestKickers)).ToList();
        }
        if (finalWinners.Count == 1) display.ShowWinner(finalWinners.First(), bestRank, potAmount);
        else
        {
            display.ShowSplitPot(finalWinners, bestRank, potAmount);
        }
        controller.AwardPot(finalWinners);
    }
    
    static int GetPlayerCount(int min, int max)
    {
        while (true)
        {
            Console.Write($"Masukkan jumlah pemain ({min}-{max}): ");
            if (int.TryParse(Console.ReadLine(), out int count) && count >= min && count <= max)
                return count;
            {
                display.ShowMessage("Input tidak valid.", ConsoleColor.Red);
            }
        }
    }
    static int GetPlayerChips(int playerNumber, int defaultChips)
    {
        while (true)
        {
            Console.Write($"Masukkan chip untuk Pemain {playerNumber} (default: {defaultChips}): ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                return defaultChips;
            if (int.TryParse(input, out int chips) && chips > controller.minRaiseAmount*2)
                return chips;
            display.ShowMessage($"Input tidak valid. Chip harus lebih besar dari Big Blind ({controller.minRaiseAmount}).", ConsoleColor.Red);
        }
    }
    
}

public class ListRankComparer : IComparer<List<CardValue>>
{
    public int Compare(List<CardValue>? x, List<CardValue>? y)
    {
        int result = 0;
        if (x != null && y != null)
        {
            for (int i = 0; i < Math.Min(x.Count, y.Count); i++)
            {
                result = y[i].CompareTo(x[i]);
                if (result != 0)
                {
                    break;
                }
            }
            if (result == 0)
            {
                result = y.Count.CompareTo(x.Count);
            }
        }
        return result;
    }
}