// See https://aka.ms/new-console-template for more information
//Inheritances
namespace Task
{
    class Fly
    {
        public string kind = "Black";
        public void voice()
        {
            Console.WriteLine("hawk, hawk");
        }
    }

    class Bird : Fly
    {
        public string birdName = "Sparrow";
    }

    class Program
    {
        static void Main(string[] args)
        {
            Bird myBird = new Bird();
            myBird.voice();

            Console.WriteLine(myBird.kind + " "+ myBird.birdName);
        }
    }
}
