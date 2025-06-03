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

Turn the generator logic in the code you have so far into a class object and make it so that the client code can configure its own rules i.e. 
add the following API myClass.AddRule(int input, string output) */


namespace FinalLogicExercise
{
class LogicGenerator
    {
        private List<KeyValuePair<int, string>> _rules;
        public LogicGenerator()
        {
            _rules = new List<KeyValuePair<int, string>>();
        }

        public void AddRule(int input, string output)
        {
            _rules.Add(new KeyValuePair<int, string>(input, output));
        }

        public void Result(int n)
        {
            for (int i = 1; i <= n; i++)
            {
                string output = "";

                List<KeyValuePair<int, string>> sortedRules = _rules.OrderBy(rule => rule.Key).ToList();

                foreach (var rule in sortedRules)
                {
                    if (i % rule.Key == 0)
                    {
                        output += rule.Value;
                    }
                }

                if (string.IsNullOrEmpty(output))
                {
                    Console.Write(i + ", ");
                }
                else
                {
                    Console.Write(output + ", ");
                }
            }
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            LogicGenerator myClass = new LogicGenerator();

            int n = 105;

            myClass.AddRule(3, "foo");
            myClass.AddRule(4, "baz");
            myClass.AddRule(5, "bar");
            myClass.AddRule(7, "jazz");
            myClass.AddRule(9, "huzz");
  
            myClass.Result(n);
        }
    }
}

