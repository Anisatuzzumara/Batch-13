// See https://aka.ms/new-console-template for more information

using System;
using System.IO;
using System.Text;
namespace FileDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //File create - Set the File Path
            string FilePath = @"C:\Users\Anisazumara\Documents\FileCoba.txt";
            //FileStream fileStream = new FileStream(FilePath, FileMode.Create);
            //fileStream.Close();
            //Console.Write("File has been created and the Path is C:\\FileCoba.txt");
            //Console.ReadKey();

            // File Open and File Write
            //FileStream fileStream = new FileStream(FilePath, FileMode.Append);
            //byte[] bdata = Encoding.Default.GetBytes("The Programming Language C# is so cool");
            //fileStream.Write(bdata, 0, bdata.Length);
            //fileStream.Close();
            //Console.WriteLine("Successfully saved file with data");
            //Console.ReadKey();

            // File Read
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
