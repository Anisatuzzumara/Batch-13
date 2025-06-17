using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryManagementMVC.Data; 
using LibraryManagementMVC.Models; 

namespace LibraryManagementMVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context; 

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Books
        public async Task<IActionResult> Index(string searchString)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Books' is null.");
            }

            var books = from b in _context.Books.Include(b => b.Publisher)
                        select b;

            if (!string.IsNullOrEmpty(searchString))
            {
                // Mencari berdasarkan judul buku atau nama penulis
                books = books.Where(s => s.Title.ToUpper().Contains(searchString.ToUpper()) 
                                      || s.Author.ToUpper().Contains(searchString.ToUpper()));
            }

            ViewData["CurrentFilter"] = searchString;
            return View(await books.ToListAsync());
        }

        // GET: /Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var book = await _context.Books
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(m => m.BookId == id);
            
            if (book == null) return NotFound();

            return View(book);
        }

        // GET: /Books/Create
        public IActionResult Create()
        {
            
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name");
            return View();
        }

        // POST: /Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Author,ISBN,PublishedYear,PublisherId")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name", book.PubId);
            return View(book);
        }

        // GET: /Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();
            
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name", book.PubId);
            return View(book);
        }

        // POST: /Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,ISBN,PublishedYear,PublisherId")] Book book)
        {
            if (id != book.BookId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name", book.PubId);
            return View(book);
        }

        // GET: /Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var book = await _context.Books
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(m => m.PubId == id);
            
            if (book == null) return NotFound();

            return View(book);
        }

        // POST: /Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return (_context.Books?.Any(e => e.BookId == id)).GetValueOrDefault();
        }
    }
}
