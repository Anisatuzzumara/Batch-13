// See https://aka.ms/new-console-template for more information

// Polymorphism
using System; 
namespace SeaAnimals
{
    class Mamalia
    {
        public virtual void animalBreathe()
        {
            Console.WriteLine("Some of the sea animal is a mamals and breathing with lungs");
        }
    }

    class Whale : Mamalia
    {
        public override void animalBreathe()
        {
            Console.WriteLine("The Whales use Lungs");
        }
    }

    class Dolphin : Mamalia
    {
        public override void animalBreathe()
        {
            Console.WriteLine("The Dolphins use lungs");
        }
    }

    class MyApp
    {
        static void Main(string[] args)
        {
            Mamalia inSea = new Mamalia();
            Mamalia inWhale = new Whale();
            Mamalia inDolphin = new Dolphin();

            inSea.animalBreathe();
            inWhale.animalBreathe();
            inDolphin.animalBreathe();
        }
    }
}