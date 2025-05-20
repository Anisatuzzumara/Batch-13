// See https://aka.ms/new-console-template for more information


// Try XXX Pattern
using System;

string input = "1235869";
int number;
bool success = int.TryParse(input, out number);

if (success)
{
    Console.WriteLine($"Conversion succeeded: {number}");
    //output conversion succeded
}
else
{
    Console.WriteLine("Conversion failed.");
    // output conversion failed
}
    




