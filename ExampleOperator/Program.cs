// See https://aka.ms/new-console-template for more information


using System;
namespace BinaryOverload {
    
class Calculator {
    
    public int number = 0;
    
    public Calculator() {}
    
    public Calculator(int n)
    {
        number = n;
    }
    
    // Overloading of Binary "+" operator
    public static Calculator operator + (Calculator Calc1, 
                                         Calculator Calc2)
    {
        Calculator Calc3 = new Calculator(0);
        Calc3.number = Calc2.number + Calc1.number;
        return Calc3;
    }
    
    // function to display result
    public void display()
    {
        Console.WriteLine("{0}", number);
    }
}


class CalNum {
    
    // Driver Code
    static void Main(string[] args)
    {
        
        Calculator num1 = new Calculator(124);
        Calculator num2 = new Calculator(90);
        Calculator num3 = new Calculator();
        
        
        num3 = num1 + num2;
        
        num1.display(); // Displays 200
        
        num2.display(); // Displays 40
        
        num3.display(); // Displays 240
        
    }
}
}
