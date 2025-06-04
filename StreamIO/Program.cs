// See https://aka.ms/new-console-template for more information

using System.IO;
using System.IO.Pipes;
using System.Text;

/*
// FileStream - File System Gateaway
using (FileStream fs = new FileStream("coba.txt", FileMode.Create))
{
    // You have complete control over how data is read/written
    byte[] data = Encoding.UTF8.GetBytes("Selamat datang di dunia pemrograman C#!\n");
    fs.Write(data, 0, data.Length);
} 

// Memory Stream - In Memory Data Processing

// All operations happen in RAM - super fast!
string conToh = "Contoh data dari Data Kecil\n";

byte[] data = Encoding.UTF8.GetBytes(conToh);

using (MemoryStream ms = new MemoryStream())
{

    ms.Write(data, 0, data.Length);
    ms.Position = 0; // Reset to read from beginning
    // Process the data...
    Console.WriteLine($"Ukuran data: {data.Length} bytes");
}


// Buffered Stream - Performance Optimization
using (FileStream fs = new FileStream("file.dat", FileMode.Create))
using (BufferedStream bs = new BufferedStream(fs, 8192)) // 8KB buffer
{
    // Small reads/writes are now buffered - much faster!
    for (int i = 0; i < 6000; i++)
    {
        bs.WriteByte((byte)i); // This would be slow without buffering
    }
} 


// Text Data for Human Readable Data
// StreamReader/Writer - Your text processing workhorses
using (FileStream fs = new FileStream("contoh.txt", FileMode.Create))
using (StreamWriter writer = new StreamWriter(fs, Encoding.UTF8))
    {
        writer.WriteLine("Today is Wednesday and Tomorrow is Thursday"); // Much easier than byte arrays!
        writer.WriteLine($"Timestamp: {DateTime.Now}");
    }

// StringReader/Writer - For in-memory text processing
string textData = "Line 1\nLine 2\nLine 3";
using (StringReader reader = new StringReader(textData))
    {
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            Console.WriteLine($"Processing: {line}");
        }
    }


// Binary Adapter - For Processing Data
// BinaryReader/Writer - Perfect for saving/loading structured data
using (FileStream fs = new FileStream("player_football.dat", FileMode.Create))
using (BinaryWriter writer = new BinaryWriter(fs))
    {
        writer.Write(7);                   // Player number (int)
        writer.Write("Christiano Ronaldo"); // Player name (string)
        writer.Write("Real Madrid");       // Team name (string)
        writer.Write(35);                  // Player age (int)
        writer.Write(true);                 // Game completed (bool)
        writer.Write(80.0f);                // Health percentage (float)
    }


// Inter-Process Communication (mutex)
// Named Pipes - Like a private telephone line between applications
using (NamedPipeServerStream pipeServer = new NamedPipeServerStream("PipeRy"))
{
    Console.WriteLine("Waiting for stable connection...");
    pipeServer.WaitForConnection();

    // Now you can send data to another application!
    StreamWriter writer = new StreamWriter(pipeServer);
    writer.WriteLine("Hello from the another world!");
    writer.Flush();
}


// Streams are NOT thread-safe by default
FileStream unsafeStream = new FileStream("coba.txt", FileMode.Open);

// Make it thread-safe
Stream safeStream = Stream.Synchronized(unsafeStream);

// Now multiple threads can safely access it
*/

// Sometimes you need specialized behavior
public class UpperCaseStream : Stream
{
    private readonly Stream baseStream;

    public UpperCaseStream(Stream stream)
    {
        baseStream = stream;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        // Convert to uppercase before writing
        string text = Encoding.UTF8.GetString(buffer, offset, count);
        byte[] upperData = Encoding.UTF8.GetBytes(text.ToUpper());
        baseStream.Write(upperData, 0, upperData.Length);
    }

    public override bool CanRead => baseStream.CanRead;
    public override bool CanSeek => baseStream.CanSeek;
    public override bool CanWrite => baseStream.CanWrite;
    public override long Length => baseStream.Length;

    public override long Position
    {
        get => baseStream.Position;
        set => baseStream.Position = value;
    }

    public override void Flush() => baseStream.Flush();

    public override int Read(byte[] buffer, int offset, int count)
        => baseStream.Read(buffer, offset, count);

    public override long Seek(long offset, SeekOrigin origin)
        => baseStream.Seek(offset, origin);

    public override void SetLength(long value)
        => baseStream.SetLength(value);

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            baseStream.Dispose();
        base.Dispose(disposing);
    }
}

class Program
{
    static void Main()
    {
        // Contoh penggunaan UpperCaseStream:
        using (FileStream fs = new FileStream("example.txt", FileMode.Create))
        using (UpperCaseStream upperStream = new UpperCaseStream(fs))
        {
            byte[] data = Encoding.UTF8.GetBytes("hello, stream customization!\n");
            upperStream.Write(data, 0, data.Length);
            upperStream.Flush();
        }
    }
}



