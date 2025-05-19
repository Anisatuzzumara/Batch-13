// See https://aka.ms/new-console-template for more information

using System;

namespace DelegateExample
{
    //Mendefinisikan delegate
    delegate int Operation(int a, int b);

    class Calculator
    {
        // Method yang sesuai dengan signature delegate
        public static int Add(int x, int y)
        {
            return x + y;
        }

        public static int Subtract(int x, int y)
        {
            return x - y;
        }

        public static int Multiply(int x, int y)
        {
            return x * y;
        }

        public static int Divide(int x, int y)
        {
            return y != 0 ? x / y : 0;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //Assign method ke delegate
            Operation op;

            op = Calculator.Add;
            Console.WriteLine("Tambah: 10 + 5 = " + op(10, 5));

            op = Calculator.Subtract;
            Console.WriteLine("Kurang: 10 - 5 = " + op(10, 5));

            op = Calculator.Multiply;
            Console.WriteLine("Kali: 10 * 5 = " + op(10, 5));

            op = Calculator.Divide;
            Console.WriteLine("Bagi: 10 / 5 = " + op(10, 5));

            Console.ReadLine();
        }
    }
}

