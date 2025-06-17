using AutoMapper;
using LibraryManagementMVC.DTOs;
using LibraryManagementMVC.Infrastructure.Repositories;
using LibraryManagementMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementMVC.Controllers
{
    public class PublishersController : Controller
    {
        private readonly IPublisherRepository _repo;
        private readonly IMapper _mapper;
        public PublishersController(IPublisherRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var publishers = await _repo.GetAllAsync();
            var publisherDtos = _mapper.Map<IEnumerable<PublisherDto>>(publishers);
            return View(publisherDtos);
        }

        public async Task<IActionResult> Details(int id)
        {
            var publisher = await _repo.GetByIdAsync(id);
            if (publisher == null)
            {
                ViewBag.Message = $"Publisher with ID {id} was not found.";
                return View("NotFound");
            }
            var dto = _mapper.Map<PublisherDto>(publisher);
            return View(dto);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Country")] Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                await _repo.AddAsync(publisher);
                await _repo.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(publisher);
        }
        
        // GET: Publishers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var publisher = await _repo.GetByIdAsync(id);
            if (publisher == null)
            {
                ViewBag.Message = $"Publisher with ID {id} was not found.";
                return View("NotFound");
            }
            var dto = _mapper.Map<PublisherCreateUpdateDto>(publisher);
            return View(dto);
        }

        // POST: Publishers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PublisherCreateUpdateDto dto)
        {
            if (id != dto.PubId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var publisher = _mapper.Map<Publisher>(dto);
                    _repo.Update(publisher);
                    await _repo.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle concurrency issues if necessary
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        // GET: Publishers/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var publisher = await _repo.GetByIdAsync(id);
            if (publisher == null)
            {
                ViewBag.Message = $"Publisher with ID {id} was not found.";
                return View("NotFound");
            }
            var dto = _mapper.Map<PublisherDto>(publisher);
            return View(dto);
        }

        // POST: Publishers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var publisher = await _repo.GetByIdAsync(id);
            if (publisher != null)
            {
                try
                {
                    _repo.Delete(publisher);
                    await _repo.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    // Handle error if publisher cannot be deleted (e.g., has related books)
                    TempData["ErrorMessage"] = $"The publisher '{publisher.Name}' cannot be deleted as it has related books.";
                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}