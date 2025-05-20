// See https://aka.ms/new-console-template for more information

using System;
//operator overloading


//Overloading the + Operator
public struct Note
{
    int value;


    public Note(int semitonesFromA)
    {
        value = semitonesFromA;
    }


    // Overload the + operator to add an integer (semitones) to a Note
    public static Note operator +(Note x, int semitones)
    {
        return new Note(x.value + semitones);
    }

    static void Main(string[] args)
    {
        Note B = new Note(2);    // Note B, 2 semitones from A
        Note CSharp = B + 2;     // Adds 2 semitones to B, resulting in C
    }
    
}


//Compound Assignment Operators
// CSharp += 2;  // Adds 2 semitones to the current CSharp note


//Expression-Bodied Operators
public static Note operator + (Note x, int semitones)
    => new Note(x.value + semitones);



//Overloading Equality (==) and Comparison (<)
public struct Note : IComparable<Note>
{
    int value;


    public Note(int semitonesFromA)
    {
        value = semitonesFromA;
    }


    // Overload the == operator
    public static bool operator ==(Note x, Note y)
    {
        return x.value == y.value;
    }


    // Overload the != operator
    public static bool operator !=(Note x, Note y)
    {
        return x.value != y.value;
    }

    // Overload the < operator
    public static bool operator <(Note x, Note y)
    {
        return x.value < y.value;
    }


    // Implement IComparable
    public int CompareTo(Note other)
    {
        return this.value.CompareTo(other.value);
    }

    public override bool Equals(object obj)
    {
        if (obj is Note)
        {
            return this == (Note)obj;
        }
        return false;
    }


    public override int GetHashCode()
    {
        return value.GetHashCode();
    }
}


// Implicit and Explicit Conversion Between Note and double (Frequency)
public struct Note
{
    int value;

    public Note(int semitonesFromA)
    {
        value = semitonesFromA;
    }


    // Implicit conversion from Note to double (frequency in Hertz)
    public static implicit operator double(Note x)
    {
        return 440 * Math.Pow(2, (double)x.value / 12);  // Convert to frequency
    }


    // Explicit conversion from double (frequency) to Note
    public static explicit operator Note(double x)
    {
        int semitones = (int)(0.5 + 12 * (Math.Log(x / 440) / Math.Log(2)));  // Convert to nearest semitone
        return new Note(semitones);
    }
}


// implicitly convert a Note to a double (frequency) and explicitly convert a double back to a Note:
Note n = new Note(2);        // 2 semitones from A
double frequency = n;        // Implicit conversion to frequency (double)
Console.WriteLine(frequency);  // Output: Frequency of the note
double freq = 554.37;        // Frequency of a note (in hertz)
Note n2 = (Note)freq;       // Explicit conversion from frequency to Note
