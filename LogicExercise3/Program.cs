// See https://aka.ms/new-console-template for more information
/*
Write a simple console program that prints the number from 1 to n, for each number x:

print "foo", if x is divisible by 3
print "bar", if x is divisible by 5
print "foobar", if x is divisible by 3 and 5

print the number itself, if x satisfies none of the rule
Here's a sample output of such program with n=15
>> 1, 2, foo, 4, bar, foo, 7, 8, foo, bar, 11, foo, 13, 14,foobar

Continuing on the previous question. Add the following rules
print "jazz", if x is divisible by 7
This means for x = 21 x = 35 and x = 105 the program should print "foojazz", "barjazz" and "foobarjazz" respectively.

Continuing on the previous question. Using the same divisible logic, use the table below as the rules  3: "foo"  4: "baz"  5: "bar"  7: "jazz"  9: "huzz"
*/


using System;
namespace LogicExercise3
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

                if (i % 4 == 0)
                    hasil += "baz";

                if (i % 5 == 0)
                    hasil += "bar";

                if (i % 7 == 0)
                    hasil += "jazz";

                if (i % 9 == 0)
                    hasil += "huzz";

                if (hasil == "")
                {
                    Console.Write(i + ", ");
                }
                else
                {
                    Console.Write(hasil + ", ");
                }
            }
        }
    }
}

