// See https://aka.ms/new-console-template for more information

using System;
namespace GeneRic
{
    public class Generik
    {
        public void Display<TypeOfValue>(string message, TypeOfValue value)
        {
            Console.WriteLine("{0}:{1}", message, value);
        }
    }

    public class Program
    {
        public static int Main()
        {
            Generik p = new Generik();
        
            p.Display<int>("Number", 2908534);
            p.Display<char>("Character", 'A');
            p.Display<double>("Decimal", 10.9);
            return 0;
        }
    }

}

