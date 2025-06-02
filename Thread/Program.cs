// See https://aka.ms/new-console-template for more information
// See https://aka.ms/new-console-template for more information
using System.Threading;
using System.Diagnostics;
namespace CobaThread
{
	public class Program
	{
		public static void Main()
		{
			var stopwatch = new Stopwatch();
			Console.WriteLine("Printing ...");
			stopwatch.Start();
			Thread thread1 = new Thread(TugasDua);
			Thread thread2 = new Thread(Tugas);
			Thread thread3 = new Thread(TugasDua);
			Thread thread4 = new Thread(Tugas);
			thread1.Name = "2";
			thread2.Name = "4";
			thread3.Name = "6";
			thread4.Name = "8";
			
			thread1.Start();
			thread1.Join();
			thread2.Start();
			thread2.Join();
			thread3.Start();
			thread3.Join();
			thread4.Start();
			thread4.Join();
			stopwatch.Stop();

			Console.WriteLine($"Elapsed Time for {thread1.Name + thread2.Name + thread3.Name + thread4.Name} to finish = {stopwatch.ElapsedMilliseconds} ms");
			//Console.WriteLine($"{thread2.Name} Status = {thread2.IsAlive}");
			if (thread1.IsAlive) 
			{
				Console.WriteLine($"{thread1.Name} is still running");
			}
			else {Console.WriteLine($"{thread1.Name} is OFF");}
			
			if (thread2.IsAlive) 
			{
				Console.WriteLine($"{thread2.Name} is still running");
			}
			else {Console.WriteLine($"{thread2.Name} is OFF");}
			
			if (thread3.IsAlive) 
			{
				Console.WriteLine($"{thread3.Name} is still running");
			}
			else {Console.WriteLine($"{thread3.Name} is OFF");}
			
			if (thread4.IsAlive) 
			{
				Console.WriteLine($"{thread4.Name} is still running");
			}
			else {Console.WriteLine($"{thread4.Name} is OFF");}
			Console.WriteLine("OK");
		}

		public static void TugasDua()
		{
			int x = 5;
			int y = 8;
			int z = x + y;
			for (int i = 0; i < 10; i++)
			{
				Console.WriteLine(z);
				Console.WriteLine(z + (x * y));
			}
		}

		public static void Tugas()
		{
			string aha = "hello";
			string bha = "world";
			for (int i = 0; i < 20; i++)
			{
				Console.WriteLine(aha + bha);
			}
		}
	}
}

