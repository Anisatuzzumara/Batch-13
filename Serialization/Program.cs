// See https://aka.ms/new-console-template for more information

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;

/*
namespace SerialDeserialization
{
	[Serializable]
	public class Room 
	{
		public int  desks {get; set;}
		public int chairs {get; set;} 
		public int people {get; set;}
	}
	class Program 
	{
		static void Main(string[] args)
		{
			// Serialization using System.Text.Json
			Room myRoom = new Room { desks = 20, chairs = 40, people = 40 };
			string jsonString = JsonSerializer.Serialize(myRoom);
			File.WriteAllText("myRoom.json", jsonString);

			// Deserialization using System.Text.Json
			string jsonStringDe = File.ReadAllText("myRoom.json");
			Room? myRoomDe = JsonSerializer.Deserialize<Room>(jsonStringDe);
		}
	}
}
*/

using System.Xml.Serialization;

[Serializable]
public class Items
{
    public string? Description { get; set; }
    public int Price { get; set; }
    public string? Condition { get; set; }
}

class Program
{
    static void Main()
    {
        // Serialization using XML
        List<Items> items = new List<Items>
        {
            new Items { Description = "Sepeda", Price = 3020000, Condition = "Baik"},
            new Items { Description = "Sepeda", Price = 3020000, Condition = "Baik"}
        };

        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Items>));

        using (StreamWriter streamWriter = new StreamWriter("items.xml"))
        {
            xmlSerializer.Serialize(streamWriter, items);
            Console.WriteLine("Items serialized to items.xml");
        }

        // Deserialization using XML
        using (StreamReader streamReader = new StreamReader("items.xml"))
        {
            List<Items> deserializedItems = (List<Items>)xmlSerializer.Deserialize(streamReader);
            Console.WriteLine(deserializedItems.Count);
            foreach (var items2 in deserializedItems)
            {
                Console.WriteLine($"Deserialize items: {items2.Description}, {items2.Price}, {items2.Condition}");
            }
        }
    }
}