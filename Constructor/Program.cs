// See https://aka.ms/new-console-template for more information

// Constructor
using System;
namespace Identifier
{
    class Fruit
    {
        public string size = "big";

        public Fruit() //class constructor
        {
            size = "big";
        }
        static void Main (string[] args)
        {
            Fruit Watermelon = new Fruit();// Create an object of the Car Class (this will call the constructor)
            Console.WriteLine(Watermelon.size);
        }
    }
}

