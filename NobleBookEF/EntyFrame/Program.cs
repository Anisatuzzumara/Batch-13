using EntyFrame.Data;
using EntyFrame.Models;

using EntyFrame _context = new DbBookContext();
/* OR:
    using (var _context =  new BooksAndNobleContext()) 
    {
        ...
        *Add/save stuff to database...*
        ...
    }
*/

static void AddSampleBookstoDb(BooksAndNobleContext context)
{
    Book scythe_NealShusterman = new Book()
    {
        Title = "Scythe",
        Author = "Neal Shusterman",
        Genre = "Science Fiction",
        Price = 10.99M
    };
    context.Add(scythe_NealShusterman); // context will implicitly detect which entity to add records to
    // OR: _context.Books.Add(scythe_NealShusterman); 


    Book wayOfKings_BrandonSanderson = new Book()
    {
        Title = "The Way of Kings",
        Author = "Brandon Sanderson",
        Genre = "Fantasy",
        Price = 7.99M
    };
    context.Add(wayOfKings_BrandonSanderson);


    Book sapiens_YuvalNoahHarari = new Book()
    {
        Title = "Sapiens: A Brief History of Humankind",
        Author = "Yuval Noah Harari",
        Genre = "Non-Fiction",
        Price = 9.99M
    };
    context.Add(sapiens_YuvalNoahHarari);

    context.SaveChanges();
}

static void BooksMoreThan9Dollars(BooksAndNobleContext context)
{ 
    // FluentAPI LINQ syntax
    var results = context.Books.Where(b => b.Price > 9).ToList();
    /* OR: Query expression LINQ syntax
    var results = from book in context.Books
                  where book.Price > 9
                  select book;
    */

    Console.WriteLine($"Books that are more than $9:");
    foreach (var result in results)
    {
        Console.WriteLine($"{result.Title} by {result.Author}");
    }
    /* OUTPUT:
    Books that are more than $9:
    Scythe by Neal Shusterman
    Sapiens: A Brief History of Humankind by Yuval Noah Harari
    */
}

static void UpdatePrice(BooksAndNobleContext context, string bookTitle, string authorName, decimal newPrice)
{
    var result = context.Books
                  .Where(
                    b => b.Title.Contains(bookTitle) &&
                    b.Author.Contains(authorName))
                  .FirstOrDefault();

    if (result == null)
    {
        Console.WriteLine("No results found.");
        return;
    }

    decimal oldPrice = result.Price;

    if (result is Book)
    {
        result.Price = newPrice;
    }

    context.SaveChanges();

    Console.WriteLine($"New Price alert for {result.Title} by {result.Author}: From ${oldPrice} to ${newPrice}");
}


//AddSampleBookstoDb(_context);

Console.WriteLine("Old Prices:");
BooksMoreThan9Dollars(_context);
UpdatePrice(_context, "The Way of Kings", "Brandon Sanderson", 11.99M);

Console.WriteLine("\nUpdated Prices:");
BooksMoreThan9Dollars(_context);


