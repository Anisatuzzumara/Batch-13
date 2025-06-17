using AutoMapper;
using LibraryManagementMVC.DTOs;
using LibraryManagementMVC.Infrastructure.Repositories;
using LibraryManagementMVC.Models;
using LibraryManagementMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementMVC.Controllers
{
    public class PublishersController : Controller
    {
        private readonly IPublisherService _publisherService;
        private readonly IMapper _mapper;

        public PublishersController(IPublisherService publisherService, IMapper mapper)
        {
            _publisherService = publisherService;
            _mapper = mapper;
        }

        // GET: /Publishers
        public async Task<IActionResult> Index(string searchString)
        {
            var publishers = await _publisherService.SearchPublishersAsync(searchString ?? string.Empty);
            var viewModels = _mapper.Map<IEnumerable<PublisherDto>>(publishers);
            ViewData["CurrentFilter"] = searchString;
            return View(viewModels);
        }

        // GET: /Publishers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var publisher = await _publisherService.GetPublisherByIdAsync(id.Value);
            if (publisher == null) return NotFound();

            var viewModel = _mapper.Map<PublisherDto>(publisher);
            return View(viewModel);
        }

        // GET: /Publishers/Create
        public IActionResult Create()
        {
            // Mengirim DTO kosong yang spesifik untuk operasi Create
            return View(new PublisherCreateDto());
        }

        // POST: /Publishers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PublisherCreateDto viewModel)
        {
            if (ModelState.IsValid)
            {
                var publisher = _mapper.Map<Publisher>(viewModel);
                await _publisherService.CreatePublisherAsync(publisher);
                TempData["SuccessMessage"] = "Penerbit berhasil ditambahkan!";
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: /Publishers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var publisher = await _publisherService.GetPublisherByIdAsync(id.Value);
            if (publisher == null) return NotFound();

            // Mengirim DTO yang spesifik untuk operasi Update
            var viewModel = _mapper.Map<PublisherUpdateDto>(publisher);
            return View(viewModel);
        }

        // POST: /Publishers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PublisherUpdateDto viewModel)
        {
            if (id != viewModel.PubId) return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var publisher = _mapper.Map<Publisher>(viewModel);
                    await _publisherService.UpdatePublisherAsync(publisher);
                    TempData["SuccessMessage"] = "Penerbit berhasil diperbarui!";
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception)
                {
                    // Menangani jika terjadi error saat update
                    ModelState.AddModelError("", "Gagal menyimpan perubahan. Silakan coba lagi.");
                }
            }
            return View(viewModel);
        }

        // GET: /Publishers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var publisher = await _publisherService.GetPublisherByIdAsync(id.Value);
            if (publisher == null) return NotFound();

            var viewModel = _mapper.Map<PublisherDto>(publisher);
            return View(viewModel);
        }

        // POST: /Publishers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _publisherService.DeletePublisherAsync(id);
            if (success)
            {
                TempData["SuccessMessage"] = "Penerbit berhasil dihapus!";
            }
            else
            {
                TempData["ErrorMessage"] = "Gagal menghapus penerbit. Mungkin masih digunakan oleh satu atau beberapa buku.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}