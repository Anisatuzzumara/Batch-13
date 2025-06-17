using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagementMVC.Repository;
using LibraryManagementMVC.Infrastructure.Repositories;
using LibraryManagementMVC.Models;

namespace LibraryManagementMVC.Services;
public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepo;
        private readonly IPublisherRepository _publisherRepo;

        public BookService(IBookRepository bookRepo, IPublisherRepository publisherRepo)
        {
            _bookRepo = bookRepo;
            _publisherRepo = publisherRepo;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _bookRepo.GetAllAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _bookRepo.GetByIdAsync(id);
        }

        public async Task CreateBookAsync(Book book)
        {
            await _bookRepo.AddAsync(book);
            await _bookRepo.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(Book book)
        {
            _bookRepo.Update(book);
            await _bookRepo.SaveChangesAsync();
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await _bookRepo.GetByIdAsync(id);
            if (book == null)
            {
                return false;
            }
            _bookRepo.Delete(book);
            await _bookRepo.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Mengambil daftar publisher untuk ditampilkan di dropdown form.
        /// Ini adalah contoh di mana service layer bisa berkoordinasi dengan repository lain.
        /// </summary>
        public async Task<IEnumerable<SelectListItem>> GetPublishersForDropdownAsync()
        {
            var publishers = await _publisherRepo.GetAllAsync();
            return publishers.Select(p => new SelectListItem
            {
                Value = p.PubId.ToString(),
                Text = p.Name
            });
        }

    Task<IEnumerable<Book>> IBookService.GetAllBooksAsync()
    {
        throw new NotImplementedException();
    }

    Task<Book?> IBookService.GetBookByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

}
