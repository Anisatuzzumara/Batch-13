// See https://aka.ms/new-console-template for more information
using System;
namespace OoP
{
    class Car
    {
        // Properti
        public string brand;
        public int year;

        // Method
        public void klakson()
        {
            Console.WriteLine("Peep! Peep!");
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Brand: {brand}, Year: {year}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Car myCar = new Car();
            myCar.brand = "Mercedez Benz";
            myCar.year = 2020;

            myCar.klakson();         
            myCar.ShowInfo();    
        }
    }
}
