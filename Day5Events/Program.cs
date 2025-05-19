// See https://aka.ms/new-console-template for more information

using System;
public delegate void ButtonEvent(object broadcast, EventArgs e);

public class Button
{
    public event ButtonEvent Handler;
    // in the class that raises the event, create a method to invoke the event

    public void OnClicked()
    {
        if (Handler != null)
        {
            // Invoke the event by calling the delegate
            Handler(this, EventArgs.Empty);
        }
    }
}

public class From
{
    public void Clicked(object broadcast, EventArgs e)
    {
        Console.WriteLine("Button was clicked!");
    }
}

public static class Program
{
    // In the main program, create an instance of the Button class and subscribe to the Clicked event.
    public static void Main()
    {
        From from = new From();
        Button button = new Button();
        button.Handler += from.Clicked;
        // Finally, raise the event by calling the event invoker method
        button.OnClicked();
    }
}


