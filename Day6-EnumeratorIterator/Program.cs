// See https://aka.ms/new-console-template for more information

//ENumeration
// Foreach
using System;

foreach (char c in "bear")
{
    Console.WriteLine(c); // output b
                          //        e
                          //        a
                          //        r
}




//Low Level Iteration with Enumerator
using (var enumerator = "bear".GetEnumerator())
{
    while (enumerator.MoveNext())
    {
        var element = enumerator.Current;
        Console.WriteLine(element); // output b
                                    //        e
                                    //        a
                                    //        r
    }
}




//Collection Initializers and Collection Expressions
// c# old
var list = new List<int> { 1, 2, 3 };
foreach (var item in list)
{
    Console.WriteLine(item); // output  1
                             //         2
                             //         3
}
// c# 12 == List<int> list = [1, 2, 3];




var dict = new Dictionary<int, string>()
{
    { 5, "five" },
    { 10, "ten" }
};
Console.WriteLine(dict[5]); // output ten
Console.WriteLine(dict[10]); // output five
// c# 12 == var dict = [5] = "five", [10] = "ten";

//Iterators yield return (lazy)
IEnumerable<int> Fibs(int fibCount)
{
    int prevFib = 0, curFib = 1;
    for (int i = 0; i < fibCount; i++)
    {
        yield return prevFib;
        int newFib = prevFib + curFib;
        prevFib = curFib;
        curFib = newFib;
    }
}


foreach (int fib in Fibs(6))
{
    Console.Write(fib + " "); // output 0 1 1 2 3 5
}



// Iterator Semantic
IEnumerable<string> Foo(bool breakEarly)
{
    yield return "One";
    yield return "Two";
    if (breakEarly)
        yield break;  // Exit early
    yield return "Three";
}


foreach (string s in Foo(true))
{
    Console.WriteLine(s);  // Output: One, Two
}

