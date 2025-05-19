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



