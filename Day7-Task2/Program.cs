// See https://aka.ms/new-console-template for more information
using System;
namespace Task
{
    class Program
    {
        static void Main()
        {
            int n = 105;

            for (int i = 1; i <= n; i++)
            {
                string hasil = "";

                if (i % 3 == 0)
                hasil += "foo";

                if (i % 5 == 0)
                hasil += "bar";

                if (i % 7 == 0)
                hasil += "jazz";

                if (hasil == "")
                hasil = i.ToString();
                
                Console.Write(hasil);

                if (i != n)
                    Console.Write(", ");
        }
            Console.WriteLine();
    }
}

}


