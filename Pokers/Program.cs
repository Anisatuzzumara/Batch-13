using Poker.Classes;
using Poker.Enumerations;
using Poker.Games;
using Poker.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Program
{
    static ITable table = new Table();
    static IPot pot = new Pot(0);
    static GameController controller = null!;

    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Title = "Texas Hold'em Poker Console Game";
        Console.WriteLine("=== Texas Hold'em Poker Console Game ===");

        int playerCount = GetPlayerCount(2, 6);

        IDeck deck = new Deck();
        controller = new GameController(10, 20, 20, deck, table, pot);

        List<Player> players = new List<Player>();
        for (int i = 1; i <= playerCount; i++)
        {
            Console.Write($"Enter name for Player {i}: ");
            string name = Console.ReadLine() ?? $"Player{i}";
            if (string.IsNullOrWhiteSpace(name)) name = $"Player{i}";
            int chips = GetPlayerChips(i, 500);
            var newPlayer = new Player(name, chips);
            players.Add(newPlayer);
            controller.SeatPlayer(newPlayer);
        }

        bool playAgain = true;
        while (playAgain)
        {
            Console.Clear();
            Console.WriteLine("\n--- Starting New Hand ---");
            controller.StartGame();

            Console.WriteLine("\n--- Initial Player Status & Hands ---");
            PrintPlayers(controller.GetSeatedPlayers());
            ShowPlayerHandsForRoundStart(controller.GetSeatedPlayers());

            controller.PostBlinds();
            Console.WriteLine("\n--- Blinds Posted ---");
            PrintPlayers(controller.GetSeatedPlayers());
            Console.WriteLine($"Pot: {pot.GetAmount()}");

            // Pre-Flop Betting
            if (ActivePlayerCount(controller) > 1)
            {
                Console.WriteLine("\n--- Pre-Flop Betting Round ---");
                ManualBettingRound(BettingRoundType.PreFlop);
            }
            PrintPlayers(controller.GetSeatedPlayers());

            // Flop
            if (ActivePlayerCount(controller) > 1)
            {
                Console.WriteLine("\n--- Dealing Flop ---");
                controller.DealCommunityCards(BettingRoundType.Flop);
                PrintCommunityCards(table);
                if (ActivePlayerCount(controller) > 1)
                {
                    Console.WriteLine("\n--- Flop Betting Round ---");
                    ManualBettingRound(BettingRoundType.Flop);
                }
            }
            PrintPlayers(controller.GetSeatedPlayers());

            // Turn
            if (ActivePlayerCount(controller) > 1)
            {
                Console.WriteLine("\n--- Dealing Turn ---");
                controller.DealCommunityCards(BettingRoundType.Turn);
                PrintCommunityCards(table);
                if (ActivePlayerCount(controller) > 1)
                {
                    Console.WriteLine("\n--- Turn Betting Round ---");
                    ManualBettingRound(BettingRoundType.Turn);
                }
            }
            PrintPlayers(controller.GetSeatedPlayers());

            // River
            if (ActivePlayerCount(controller) > 1)
            {
                Console.WriteLine("\n--- Dealing River ---");
                controller.DealCommunityCards(BettingRoundType.River);
                PrintCommunityCards(table);
                if (ActivePlayerCount(controller) > 1)
                {
                    Console.WriteLine("\n--- River Betting Round ---");
                    ManualBettingRound(BettingRoundType.River);
                }
            }
            PrintPlayers(controller.GetSeatedPlayers());

            // Showdown
            PerformShowdown();

            Console.WriteLine("\n--- Final Chip Counts for the Hand ---");
            PrintPlayers(controller.GetSeatedPlayers());

            // Remove players with no chips from the game
            var brokePlayers = players.Where(p => p.GetChips() <= 0).ToList();
            foreach (var brokePlayer in brokePlayers)
            {
                Console.WriteLine($"{brokePlayer.GetName()} is out of chips and has been removed from the game.");
                controller.RemovePlayer(brokePlayer);
                players.Remove(brokePlayer);
            }

            if (players.Count < 2)
            {
                Console.WriteLine("\nNot enough players to continue. Game Over.");
                if (players.Any()) Console.WriteLine($"Last player standing: {players.First().GetName()}");
                playAgain = false;
            }
            else
            {
                Console.Write("\nPlay another hand? (Y/N): ");
                if (Console.ReadLine()?.Trim().ToUpper() != "Y")
                {
                    playAgain = false;
                }
            }
        }
        Console.WriteLine("\nThanks for playing!");
    }

    // Manual betting round for user input
    static void ManualBettingRound(BettingRoundType roundType)
    {
        var players = controller.GetSeatedPlayers().Where(p => !p.IsFolded() && p.GetChips() > 0).ToList();
        int currentBet = controller.CurrentMaxBet;
        bool bettingDone = false;
        var alreadyActed = new HashSet<IPlayer>();

        while (!bettingDone)
        {
            bettingDone = true;
            foreach (var player in players)
            {
                if (player.IsFolded() || player.GetChips() == 0) continue;

                Console.WriteLine($"\n{player.GetName()}'s turn. Chips: {player.GetChips()}, Current Bet: {player.GetCurrentBetInRound()}, To Call: {controller.CurrentMaxBet - player.GetCurrentBetInRound()}");
                Console.Write("Your hand: ");
                PrintHand(player.GetHand(), inline: true);

                string action = "";
                int toCall = controller.CurrentMaxBet - player.GetCurrentBetInRound();
                bool valid = false;
                while (!valid)
                {
                    Console.Write("Choose action ([F]old, [C]all, [R]aise, [K]Check, [A]ll-In): ");
                    action = Console.ReadLine()?.Trim().ToUpper() ?? "";
                    if (action == "F")
                    {
                        controller.HandlePlayerAction(player, ActionType.Fold, 0);
                        valid = true;
                    }
                    else if (action == "C" && toCall > 0)
                    {
                        controller.HandlePlayerAction(player, ActionType.Call, toCall);
                        valid = true;
                    }
                    else if (action == "K" && toCall == 0)
                    {
                        controller.HandlePlayerAction(player, ActionType.Check, 0);
                        valid = true;
                    }
                    else if (action == "R")
                    {
                        Console.Write("Enter raise amount (minimum raise: " + controller.MinRaise + "): ");
                        if (int.TryParse(Console.ReadLine(), out int raiseAmount) && raiseAmount >= controller.MinRaise && raiseAmount <= player.GetChips())
                        {
                            controller.HandlePlayerAction(player, ActionType.Raise, raiseAmount);
                            valid = true;
                            bettingDone = false; // Others may want to respond to raise
                        }
                        else
                        {
                            Console.WriteLine("Invalid raise amount.");
                        }
                    }
                    else if (action == "A")
                    {
                        int allInAmount = player.GetChips();
                        controller.HandlePlayerAction(player, ActionType.AllIn, allInAmount);
                        valid = true;
                        bettingDone = false; // Others may want to respond to all-in
                    }
                    else
                    {
                        Console.WriteLine("Invalid action. Try again.");
                    }
                }
                PrintPlayers(controller.GetSeatedPlayers());
                if (ActivePlayerCount(controller) <= 1) return;
            }
        }
    }

    static void PerformShowdown()
    {
        Console.WriteLine("\n--- Showdown ---");
        PrintCommunityCards(table);

        var activePlayersForShowdown = controller.GetSeatedPlayers()
                                           .Where(p => !p.IsFolded())
                                           .Select(p => p as Player)
                                           .Where(p => p != null)
                                           .ToList();

        if (!activePlayersForShowdown.Any())
        {
            Console.WriteLine("No players left for showdown.");
            return;
        }

        if (activePlayersForShowdown.Count == 1)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            var solePlayer = activePlayersForShowdown[0];
            Console.WriteLine($"\n🏆 Winner: {solePlayer.GetName()} as the only remaining player in the hand.");
            Console.ResetColor();
            controller.AwardPot(new List<IPlayer> { solePlayer });
            return;
        }

        foreach (var player in activePlayersForShowdown)
        {
            if (player == null) continue;
            Console.WriteLine($"\n{player.GetName()}'s Hand:");
            PrintHand(player.GetHand());
        }

        var winners = controller.FindWinners(
                            activePlayersForShowdown.Cast<IPlayer>().ToList(),
                            table.GetCommunityCards()
                        );

        if (winners.Any())
        {
            if (winners.Count == 1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n🏆 Winner: {winners[0].GetName()}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n🤝 It's a tie! Split pot between:");
                foreach (var winner in winners)
                {
                    Console.WriteLine($"- {winner.GetName()}");
                }
                Console.ResetColor();
            }
            controller.AwardPot(winners.Cast<IPlayer>().ToList());
        }
        else
        {
            Console.WriteLine("Error: No winners determined.");
        }
    }

    static int ActivePlayerCount(GameController activeController)
    {
        return activeController.GetSeatedPlayers().Count(p => !p.IsFolded() && p.GetChips() > 0);
    }

    static int GetPlayerCount(int min, int max)
    {
        int count;
        while (true)
        {
            Console.Write($"Enter number of players ({min}-{max}): ");
            if (int.TryParse(Console.ReadLine(), out count) && count >= min && count <= max)
                return count;
            Console.WriteLine($"Invalid number. Please enter a number between {min} and {max}.");
        }
    }

    static int GetPlayerChips(int playerNumber, int defaultChips)
    {
        int chips;
        while (true)
        {
            Console.Write($"Enter starting chips for Player {playerNumber} (default: {defaultChips}): ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) return defaultChips;
            if (int.TryParse(input, out chips) && chips > 0)
                return chips;
            Console.WriteLine("Invalid amount. Please enter a positive number for chips or press Enter for default.");
        }
    }

    static void PrintPlayers(List<IPlayer> playersToPrint)
    {
        Console.WriteLine("\n--- Player Status ---");
        if (!playersToPrint.Any())
        {
            Console.WriteLine("No players to display.");
            return;
        }
        foreach (var p in playersToPrint)
        {
            string status = p.IsFolded() ? " (Folded)" : (p.GetChips() == 0 ? " (All-In)" : "");
            string dealerMark = (p.GetPosition() == Position.Dealer) ? " (D)" : "";
            string sbMark = (p.GetPosition() == Position.Small_Blind) ? " (SB)" : "";
            string bbMark = (p.GetPosition() == Position.Big_Blind) ? " (BB)" : "";
            Console.WriteLine($"{p.GetName()}{dealerMark}{sbMark}{bbMark} - Chips: {p.GetChips()} - Bet This Round: {p.GetCurrentBetInRound()}{status}");
        }
        Console.WriteLine($"Total Pot: {pot.GetAmount()} | Current Max Bet to Call/Raise: {controller.CurrentMaxBet}");
    }

    static void ShowPlayerHandsForRoundStart(List<IPlayer> playersToShow)
    {
        Console.WriteLine("\n--- Player Hole Cards ---");
        foreach (var p in playersToShow)
        {
            if (!p.IsFolded())
            {
                Console.Write($"{p.GetName()}: ");
                PrintHand(p.GetHand(), inline: true);
            }
        }
        Console.WriteLine();
    }

    static void PrintCommunityCards(ITable gameTable)
    {
        Console.WriteLine("\n--- Community Cards ---");
        var community = gameTable.GetCommunityCards();
        if (community == null || !community.Any())
        {
            Console.WriteLine("(No community cards yet)");
        }
        else
        {
            PrintHand(community);
        }
    }

    static void PrintHand(List<ICard> cards, bool inline = false)
    {
        if (cards == null || !cards.Any())
        {
            if (inline) Console.Write("(Empty Hand)");
            else Console.WriteLine("(Empty hand)");
            return;
        }

        if (inline)
        {
            Console.WriteLine(string.Join(" ", cards.Select(c => CardShortString(c))));
            return;
        }

        List<string[]> cardVisuals = cards.Select(card => GetCardVisual(card)).ToList();
        if (!cardVisuals.Any() || cardVisuals[0] == null) return;

        for (int i = 0; i < cardVisuals[0].Length; i++)
        {
            StringBuilder lineBuilder = new StringBuilder();
            foreach (var cardLines in cardVisuals)
            {
                if (cardLines != null && i < cardLines.Length)
                {
                    lineBuilder.Append(cardLines[i] + "  ");
                }
            }
            Console.WriteLine(lineBuilder.ToString());
        }
    }
    static string CardShortString(ICard card)
    {
        string valueString;
        switch (card.Value)
        {
            case CardValue.Ace: valueString = "A"; break;
            case CardValue.King: valueString = "K"; break;
            case CardValue.Queen: valueString = "Q"; break;
            case CardValue.Jack: valueString = "J"; break;
            case CardValue.Ten: valueString = "T"; break;
            default:
                valueString = ((int)card.Rank).ToString();
                break;
        }
        string suitSymbol = card.Suit switch
        {
            Suit.Hearts => "♥", Suit.Diamonds => "♦", Suit.Clubs => "♣", Suit.Spades => "♠", _ => "?"
        };
        return valueString + suitSymbol;
    }

    static string[] GetCardVisual(ICard card)
    {
        string valueString;
        switch (card.Value)
        {
            case CardValue.Ace: valueString = "A"; break;
            case CardValue.King: valueString = "K"; break;
            case CardValue.Queen: valueString = "Q"; break;
            case CardValue.Jack: valueString = "J"; break;
            case CardValue.Ten: valueString = "T"; break;
            default:
                if ((int)card.Value >= 2 && (int)card.Value <= 9)
                    valueString = ((int)card.Value).ToString();
                else
                    valueString = "?";
                break;
        }

        string suitSymbol = card.Suit switch
        {
            Suit.Hearts => "♥", Suit.Diamonds => "♦", Suit.Clubs => "♣", Suit.Spades => "♠", _ => "?"
        };

        string valueTop = valueString.PadRight(2);
        string valueBottom = valueString.PadLeft(2);
        if (valueString == "T" || valueString == "10")
        {
            valueTop = "10"; valueBottom = "10";
        }

        return new[] {
            "┌───────┐",
            $"| {valueTop}    |",
            $"|   {suitSymbol}   |",
            $"|    {valueBottom} |",
            "└───────┘"
        };
    }
}
