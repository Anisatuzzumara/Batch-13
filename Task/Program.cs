// See https://aka.ms/new-console-template for more information

using System;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        Task<int> task = Task.Run(() =>
        {
            return 10 + 4;
        });

        task.ContinueWith((antecedent) =>
        {
            Console.WriteLine("The result is: " + antecedent.Result);  
        });

        Console.ReadLine();  
    }}
