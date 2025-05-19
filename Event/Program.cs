// See https://aka.ms/new-console-template for more information

using System;


// Declaring using event

namespace Eventt
{
    //Define a delegate that matches the event signature
    public delegate void PriceChangedHandler(decimal oldPrice, decimal newPrice);
    public class Vent
    {
        //declare an event using the delegate
        public event PriceChangedHandler PriceChanged;

        // a method to trigger the event
        public void ChangePrice(decimal newPrice)
        {
            decimal oldPrice = 100m; // Example old price
            PriceChanged?.Invoke(oldPrice, newPrice); // Fire the event
        }

    }

    // Declaring Subscribing to Events
    public class Subscribing
    {
        public void OnPriceChanged(decimal oldPrice, decimal newPrice)
        {
           // Console.WriteLine($"Price changed from {oldPrice} to {newPrice}");
        }
    }

    class Program
    {
        static void Main()
        {
            Vent broadcaster = new Vent();
            Subscribing subscriber = new Subscribing();

            // Subscribe to the event
            broadcaster.PriceChanged += subscriber.OnPriceChanged;

            // Trigger the event
            broadcaster.ChangePrice(120m);

            // Unsubscribe from the event
            broadcaster.PriceChanged -= subscriber.OnPriceChanged;
        }

        //Standard Events Pattern Penggunaan EventArgs khusus untuk menyampaikan informasiperubahan harga
        public class PriceCHangedEventArgs : EventArgs
        {
            public decimal LastPrice { get; }
            public decimal NewPrice { get; }

            public PriceCHangedEventArgs(decimal lastPrice, decimal newPrice)
            {
                LastPrice = lastPrice;
                NewPrice = newPrice;
            }
        }

    }
}


// Using EVentHandler<T> Delegate

public class Point
{
    private decimal price;
    public event EventHandler<PriceChangedEventArgs> PriceChanged;

    public decimal Price
    {
        get => price;
        set
        {
            if (price != value)
            {
                var oldPrice = price;
                price = value;
                OnPriceChanged(new PriceChangedEventArgs(oldPrice, price));
            }
        }
    }

    protected virtual void OnPriceCHanged(PriceChangedEventArgs eventArgs
    {
        PriceChanged?.Invoke(this, e);
    }
}


// Thread-Safety Considerations
protected virtual void OnPriceCHanged(PriceChangedEventArgs e)
{
    var temp = PriceChanged;
    temp? Invoke(ThreadStaticAttribute, e);
    //alternatively
    PriceChanged?.Invoke(this, e);
}


// Event Accessors
private EventHandler priceChanged;

public event EventHandler PriceChanged
{
    add
    {
        priceChanged += value;
    }
    remove
    {
        priceChanged -= value;
    }
}






