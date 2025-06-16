using BookStoreMVC.Models;
using BookStoreMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreMVC.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        // GET: Author
        public async Task<IActionResult> Index(string searchString)
        {
            IEnumerable<Author> authors;

            if (!string.IsNullOrEmpty(searchString))
            {
                authors = await _authorService.SearchAuthorAsync(searchString);
                ViewData["CurrentFilter"] = searchString;
            }
            else
            {
                authors = await _authorService.GetAllAuthorsAsync();
            }

            return View(authors);
        }

        // GET: Author/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var author = await _authorService.GetAuthorByIdAsync(id.Value);
            if (author == null) return NotFound();

            return View(author);
        }

        // GET: Author/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Author/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Author author)
        {
            if (ModelState.IsValid)
            {
                await _authorService.CreateAuthorAsync(author);
                TempData["SuccessMessage"] = "Author created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Author/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var author = await _authorService.GetAuthorByIdAsync(id.Value);
            if (author == null) return NotFound();

            return View(author);
        }

        // POST: Author/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Author author)
        {
            if (id != author.AuthorID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _authorService.UpdateAuthorAsync(author);
                    TempData["SuccessMessage"] = "Author updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Unable to save changes.");
                }
            }
            return View(author);
        }

        // GET: Author/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var author = await _authorService.GetAuthorByIdAsync(id.Value);
            if (author == null) return NotFound();

            return View(author);
        }

        // POST: Author/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _authorService.DeleteAuthorAsync(id);
            if (success)
            {
                TempData["SuccessMessage"] = "Author deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Error deleting author.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}