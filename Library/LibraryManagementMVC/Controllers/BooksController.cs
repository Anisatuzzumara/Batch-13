using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementMVC.Dtos;
using LibraryManagementMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using LibraryManagementMVC.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementMVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var books = await _context.Books.Include(b => b.Publisher).ToListAsync();
            return View(books);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            var model = new BookCreateUpdateDto
            {
                Publishers = _context.Publishers.Select(p => new SelectListItem
                {
                    Value = p.PubId.ToString(),
                    Text = p.Name
                }).ToList()
            };
            return View(model);
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookCreateUpdateDto dto)
        {
            if (ModelState.IsValid)
            {
                var book = new Book
                {
                    Title = dto.Title,
                    Author = dto.Author,
                    ISBN = dto.ISBN,
                    PublishedYear = dto.PublishedYear,
                    PubId = dto.PublisherId
                };
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Repopulate dropdown if model state is invalid
            dto.Publishers = _context.Publishers.Select(p => new SelectListItem
            {
                Value = p.PubId.ToString(),
                Text = p.Name
            }).ToList();
            return View(dto);
        }
    }
}
