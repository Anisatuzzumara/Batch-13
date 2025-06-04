// See https://aka.ms/new-console-template for more information

using System;
using System.IO;
namespace FileStreamWR
{
    class Program
    {
        static void Main(string[] args)
        {
            // This will create a file named MyFile.txt at the specified location i.e. in the D Drive
            // Here we are using the StreamWriter constructor which takes the string path as an argument to create an instance of StreamWriter class
            // StreamWriter sw = new StreamWriter("C:Fille.txt");

            //Asking user to enter the text that we want to write into the MyFile.txt file
            //Console.WriteLine("Enter the Text that you want to write on File");

            // To read the input from the user
            //string str = Console.ReadLine();

            // To write the data into the stream
            //sw.Write(str);

            // Clears all the buffers
            //sw.Flush();

            // To close the stream
            //sw.Close();
            //Console.ReadKey();

            /*
            // using Stream Writer
            string file = @"C:\Users\Anisazumara\Documents\TextFile.txt";
            int a, b, result;
            a = 100;
            b = 289;
            result = a + b;

            using (StreamWriter writer = new StreamWriter(file))
            {
                writer.Write("Sum of {0} + {1} = {2}", a, b, result);
            }
            Console.WriteLine("Saved");
            Console.ReadKey();
            

            // using Stream Reader
            StreamReader sr = new StreamReader("C:Fille.txt");

            Console.WriteLine("Content of the File");

            // This is used to specify from where to start reading input stream
            sr.BaseStream.Seek(0, SeekOrigin.Begin);

            // To read line from input stream
            string str = sr.ReadLine();

            // To read the whole file line by line
            while (str != null)
            {
                Console.WriteLine(str);
                str = sr.ReadLine();
            }
            Console.ReadLine();

            // to close the stream
            sr.Close();
            Console.ReadKey();
            */

            // using Stream Reader and Stream Writer
            string FilePath = @"C:\Users\Anisazumara\Documents\TextFile.txt";
            string data;
            FileStream fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            using (StreamReader streamReader = new StreamReader(fileStream))
            {
                data = streamReader.ReadToEnd();
            }
            Console.WriteLine(data);
            Console.ReadLine();

            


        }
    }
}
