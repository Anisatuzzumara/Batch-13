// See https://aka.ms/new-console-template for more information


using System;
using System.Threading;

class Program
{
    static void Main()
    {
        // Create a new thread that will run the WriteY method
        Thread t = new Thread(WriteY);
        t.Start();  // Starts the thread execution

        // Main thread is doing something in parallel
        for (int i = 0; i < 1000; i++) 
            Console.Write("x");

        // The WriteY method will be executed on the new thread
        void WriteY()
        {
            for (int i = 0; i < 1000; i++) 
                Console.Write("y");
        }
    }
}


Thread t = new Thread(WriteY) { Name = "MyThread" };
t.Start();
Console.WriteLine(Thread.CurrentThread.Name);  // Outputs the current thread's name
