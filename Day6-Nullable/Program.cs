// See https://aka.ms/new-console-template for more information

using System;

//Nullable Values
int? i = null;
Console.WriteLine(i == null);  // Output: True


//Nullable Struct
public struct Nullable<T> where T : struct
{
    public T Value { get; } //hold value if any
    public bool HasValue { get; }//True if value not null
    public T GetValueOrDefault();// return value atau default(T)
    public T GetValueOrDefault(T defaultValue);// return value or defaultvalue
}


int? i = null;
Console.WriteLine(i.HasValue);  // Output: False


//Implicit dan explicit nullble conversion
//implicit from T to T?
int? x = 5;


//explicit from T? to T
int? x = 5;
int y = (int)x;


// if the value null
int? x = 5;
int y = (int)x; // output = Throw InvalidOperationException because x is null


if (x.HasValue)
{
    int y = (int)x;
}


// Boxing and Unboxing NUllable values
int? x = 10;
object o = x;  // Boxing, stores the value 10


object o = "string";
int? x = o as int?;
Console.WriteLine(x.HasValue);  // Output: False
// ( in that case, string to int is failed, so x is null)


// Operator Lifting (<, >, ==, +)
int? x = 5;
int? y = 10;
bool b = x < y;  // true
Console.WriteLine(b);
// So Lifting is holds perbandingan operation
// semantik is the same with:
bool b = (x.HasValue && y.HasValue) ? (x.Value < y.Value) : false;



// Equality Operator (==, !=)
// if both of them null, they are same.
int? x = 5;
int? y = 5;
Console.WriteLine(x == y);  // Output: True


int? a = null;
Console.WriteLine(a == null);  // Output: True


//however if just one operan value is null, so both of them is not same
int? x = 5;
int? y = null;
Console.WriteLine(x == y);  // Output: False


// Relational Operator (<, >, <=, >=)
//so the operator has return false when either operand has value null:
int? x = 5;
int? y = null;
bool result = x < y;  // Output: False
Console.WriteLine(result);


// Arithmetic Operators (+, -, *, /, etc.):
int? x = 5;
int? y = null;
int? result = x + y;  // Output: null
Console.WriteLine(result);
//in thos case, if y is null, so the operation penjumlahan is null.


//Mixing Nullable and Non-Nullable Types
int? a = null;
int b = 2;
int? c = a + b;  // c is null because a is null
Console.WriteLine(c);  // Output: null


//special behaviour with bool? and logical operators
//operator & dan | hold a null as a value unknown
bool? n = null;
bool? f = false;
bool? t = true;


Console.WriteLine(n | n);  // Output: null
Console.WriteLine(n | f);  // Output: null
Console.WriteLine(n | t);  // Output: true
Console.WriteLine(n & f);  // Output: false
Console.WriteLine(n & t);  // Output: null


//The ?? Operator (Null-Coalescing Operator)
int? x = null;
int y = x ?? 5;  // If x is null, y will be 5
Console.WriteLine(y);  // Output: 5


//Operator ?? checking is the value null.
//if yes, operator will return default value yang ditentukan;
//if not, operator will return he value itself
//we can arranged ?? to give a few defaultvalue return


int? a = null, b = 1, c = 2;
Console.WriteLine(a ?? b ?? c);  // Output: 1 (first non-null value)
