// See https://aka.ms/new-console-template for more information

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.Win32.SafeHandles;

namespace Generik
{
    class Runn
    {
        delegate int Transformer(int x); // Define the delegate type
        static int Square(int x) => x * x; // a method that matches the delegate signature

        static void Main(string[] args)
        {
            Transformer t = Square; // assign the method to a delegate variable
            int result = t(9); // invoke the delegate

            Console.WriteLine(result);
        }

    }
}


// Instance and Static Method Targets
// Static

class Coba
{
    delegate int Transformer(int x);
    public static int Square(int x) => x + x;


    static void Main(string[] args)
    {
        Transformer t = Coba.Square;
        Console.WriteLine(t(10));
    }
}


// Instance 


class Test
{
    delegate int Transformer(int x);
    public int Square(int x) => x / x;

    static void Main(string[] args)
    {
        Test test = new Test();
        Transformer t = test.Square;
        Console.WriteLine(t(10));
    }
}


// Multicast Delegates

class Multicas
{
    delegate void ProgressReporter(int percentComplete);

    static void Main(string[] args)
    {
        void WriteProgressToConsole(int percentComplete)// define 1 methods
        {
            Console.WriteLine(percentComplete);
        }

        void WriteProgressToFile(int percentComplete)// define 1 methods
        {
            System.IO.File.WriteAllText("progress.txt", percentComplete.ToString());
        }

        // create multicast delegate
        ProgressReporter reporter = WriteProgressToConsole;
        reporter += WriteProgressToFile;

        reporter(50); // invoke both WriteProgressToConsole and WriteProgressToFile


    }
}


// Generic Delegate Types


class GenD
{
    delegate T Transformer<T>(T arg); // define generic delegates

    static void Main(string[] args)
    {
        static int Circle(int x) => x - x;

        int[] values = { 1, 2, 3 };// usage with a generic delegate
        Util.Transform(values, Circle);// work for any tipe of t

    }

    class Util
    {
        // The transform method can now handle any type of data
        public static void Transform<T>(T[] values, Transformer<T> transformer)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = transformer(values[i]);
            }
        }
    }
}


// using Func

class Fung
{
    static void Main(string[] args)
    {
        Func<int, int, int> add = (x, y) => x * y;
        Console.WriteLine(add(5, 3));
    }
}


// Using Action Delegates

class Act
{
    static void Main(string[] args)
    {
        Action<string> printMessage = message => Console.WriteLine(message);
        printMessage("Hello, Folks!");
    }

}


//Type Compatibility


class Compa
{
    // define 2 delegate types with identical signatures
    delegate void F1();
    delegate void F2();

    static void Main(string[] args)
    {
        // define a method to match the signature
        void Method1() { }

        //F2 f2 = f1;
        // compile error cannot convert F1 to F2, so ypu have to explicitly

        // create a F1 delegate and assign it to a F2 delegate
        F1 f1 = Method1;
        F2 f2 = new F2(f1); // explicitly creating F2 from F1

        f2(); // Works fine as Method1 is invoked
    }
}


// Multicast Delegate Equality

class Mde
{
    delegate void G();

    // define a methof to match the delegate signature
    static void Main(string[] args)
    {
        void Method2() { }

        G g1 = Method2;
        G g2 = Method2;

        Console.WriteLine(g1 == g2); // ouputs true, because both point to the same method

        // However, if the methods are in a different order, or if the delegates refer to different methods, they will not be considered equal
        G g3 = Method2;
        g3 += Method2;

        Console.WriteLine(g1 == g3); // outputs: false, because g1 and g3 reference different method chains
    }
}


// Parameter Compability
class Pc
{
    delegate void PringCation(string s);

    static void Main(string[] args)
    {
        void ActivRt(object o) => Console.WriteLine(o);

        PringCation te = new PringCation(ActivRt);

        // we can pass a string, as string is an object
        te("anisa"); 
    }
}

