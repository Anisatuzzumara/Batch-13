// See https://aka.ms/new-console-template for more information

using System;
using System.Runtime.CompilerServices;
/*
// try
{
    // Code that might throw an exception
}
catch (ExceptionTypeA ex)
{
    // Handle ExceptionTypeA
}
catch (ExceptionTypeB ex)
{
    // Handle ExceptionTypeB
}
finally
{
    // Cleanup code, runs regardless of exceptions
} */


// Basic Try-Catch

int y = Calc(0);
Console.WriteLine(y);

int Calc(int x)
{
    return 10 / x;
}
// output  Unhandled exception. System.DivideByZeroException: Attempted to divide by zero.



// Tambahkan try catch blocking
try
{
    int g = Calcc(0);
    Console.WriteLine(y);
}
catch (DivideByZeroException ex)
{
    Console.WriteLine("x cannot be zero");
}
Console.WriteLine("Program completed");

int Calcc(int f)
{
    return 10 / f;
}

// The Catch Clause

class Test
{
    static void Main(string[] args)
    {
        try
        {
            byte b = byte.Parse(args[0]);
            Console.WriteLine(b);
        }
        catch (IndexOutOfRangeException)
        {
            Console.WriteLine("Please provide at least one argument");
        }
        catch (FormatException)
        {
            Console.WriteLine("Thats not a number!");
        }
        catch (OverflowException)
        {
            Console.WriteLine("The number is too large to fit in a byte");
        }
    }
}



// Exception Filters with when
class Excep
{
    static void Main(string[] args)
    {
        try
        {
            throw new WebException("Timeout", WebExceptionStatus.Timeout);
        }
        catch (WebException ex) when (ex.Status == WebExceptionStatus.Timeout)
        {
            Console.WriteLine("Timeout occured");
        }
        catch (WebException ex) when (ex.Status == WebExceptionStatus.SendFailure)
        {
            Console.WriteLine("Send failure occured");
        }
    }
}


// Finally Block
void ReadFile()
{
    StreamReader reader = null;
    try
    {
        reader = File.OpenText("file.txt");
        if (reader.EndOfStream) return;
        Console.WriteLine(reader.ReadToEnd());
    }
    finally
    {
        if (reader != null) reader.Dispose();
    }
}
*/



// UsingStatement
using (StreamReader reader = File.OpenText("file.txt"))
{
    Console.WriteLine(reader.ReadLine());
}



// Throwing Exception


class Program
{
    void Display(string name)
    {
        if (name == null)
            throw new ArgumentNullException(nameof(name));
        Console.WriteLine(name);
    }


    public string Foo() => throw new NotImplementedException();


    static void Main()
    {
        Program p = new Program();
        p.Display("ChatGPT");
        // p.Foo(); // Akan throw NotImplementedException jika dipanggil
    }
}




//Rethrowing Exception
try
{
    // Code that may throw an exception
}
catch (Exception ex)
{
    // Log the error
    throw;  // Rethrow the same exception
}


// Alternatively
try
{
    // Parse a DateTime from some data
}
catch (FormatException ex)
{
    throw new XmlException("Invalid DateTime format", ex);
}
*/


//Try XXX  Pattern
string input = "123";
int result;
bool success = int.TryParse(input, out result);


if (success)
{
    Console.WriteLine($"Parsed successfully: {result}");
}
else
{
    Console.WriteLine("Failed to parse the input.");
}
*/





class Programm
{
    public static bool TryDivide(int numerator, int denominator, out int result)
    {
        if (denominator == 0)
        {
            result = 0;
            return false;  // Return false if division by zero
        }


        result = numerator / denominator;
        return true;
    }


    static void Main()
    {
        int result;
        bool success = TryDivide(10, 2, out result);


        if (success)
        {
            Console.WriteLine($"Division result: {result}");
        }
        else
        {
            Console.WriteLine("Division by zero occurred.");
        }
    }
}




// Return Codes
public int OpenFile(string filePath)
{
    if (string.IsNullOrEmpty(filePath))
        return -1; // Invalid file path


    if (!File.Exists(filePath))
        return -2; // File not found


    // Code to open the file
    return 0; // Success
}




public bool TryOpenFile(string filePath, out string message)
{
    if (string.IsNullOrEmpty(filePath))
    {
        message = "Invalid file path.";
        return false;
    }


    if (!File.Exists(filePath))
    {
        message = "File not found.";
        return false;
    }


    // Open the file here (dummy)
    message = "File opened successfully.";
    return true;
}




