// See https://aka.ms/new-console-template for more information

//Console.WriteLine("Hello, World!");
int n = 15;

for (int i=1;i<=n;i++)		{	
    if (i%3==0 && i%5==0){
        Console.WriteLine("FooBar"+", ");
    }
    if(i%3==0){
		Console.WriteLine("Foo"+",");			
    }
    else if (i%5==0){ 
       Console.WriteLine("Bar"+", ");			
	}
    else{
        Console.WriteLine(i+",");
    }

}


