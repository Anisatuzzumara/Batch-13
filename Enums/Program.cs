// See https://aka.ms/new-console-template for more information


using System;

namespace EnumS
{
    enum Button : byte
    {
	OFF,

	ON

    }

    public class Lamp
    {
	    static void Main(string[] args)
	    {
            byte b = 1 ;

		    if (b == (byte)Button.OFF)
		    {
			    Console.WriteLine("The Lamp is Off");
		    }
		
		    else if (b == (byte)Button.ON) 
		    {
			    Console.WriteLine("The Lamp is ON");
		    }
		
		    else
		    {
			    Console.WriteLine("Lamp cannot on or off because of byte cannot hold such large value");
		    }
	    }
    }

}

