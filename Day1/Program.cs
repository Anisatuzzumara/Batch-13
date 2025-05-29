// See https://aka.ms/new-console-template for more information
﻿using System.Collections.Generic;
namespace tesPOKER
{
    public interface IPlayer
    {
        int GetID();
        void SetID(int ID);
        string GetName();
        void SetName(string name);
        int GetChips();
        void SetChips(int chips);
    }

    public class Player : IPlayer
    {
        private int _ID;
        private string _name;
        private int _chips;
        public int GetID()
        {
            return _ID;
        }
        public string GetName()
        {
            return _name;
        }
        public int GetChips()
        {
            return _chips;
        }
        public void SetChips(int chips)
        {
            _chips = chips;
        }
        public void SetName(string name)
        {
            _name = name;
        }
        public void SetID(int ID)
        {
            _ID = ID;
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            //GameController gamecontroller = new GameController();
            Console.WriteLine(" --- Welcome to POKER game. ---");
            Console.WriteLine("Please input the number of players: ");
            int totalplayers = Convert.ToInt32(Console.ReadLine());
            IPlayer player1 = new Player();
            Console.Write("Player 1, enter your ID: ");
            player1.SetID(Convert.ToInt32(Console.ReadLine()));
            Console.WriteLine(player1.GetID());
            Console.Write("Player 1, enter your Name: ");
            player1.SetName(Convert.ToString(Console.ReadLine()));
            Console.WriteLine(player1.GetName());
            Console.Write("Player 1, enter your initial chips: ");
            player1.SetChips(Convert.ToInt32(Console.ReadLine()));
            Console.WriteLine(player1.GetChips());

            Console.Write(player1.GetName() + ", Choose your option: [F]old, [C]heck, [B]et, [D]ial, [R]aise: ");
            char flop1 = Convert.ToChar(Console.Read());
            switch (flop1)
            {
                case 'f':
                    Console.WriteLine("You have taken 'Fold' option.");
                    break;
                case 'c':
                    Console.WriteLine("You have taken 'Check' option. ");
                    break;
                case 'b':
                    Console.WriteLine("You have taken 'Bet' option.");
                    break;
                case 'd':
                    Console.WriteLine("You have taken 'Dial' option.");
                    break;
                case 'r':
                    Console.WriteLine("You have taken 'Raise' option.");
                    break;
                default:
                    Console.WriteLine("");
                    break;
            }
        }

        enum CardRank { Two, Three, Four, Five, Six, Seven, Nine, Ten, Jack, King, Ace };

        Random random = new Random();
        Type type = typeof(CardRank);
        Array values = type.GetEnumValues();
        int index = Random.Next(values.Length);
        CardRank value = Console.WriteLine((CardRank)ValueTask.GetValue(index));
    }
}
