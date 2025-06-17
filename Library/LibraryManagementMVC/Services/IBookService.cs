using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagementMVC.Models;

namespace LibraryManagementMVC.Services;

/// <summary>
    /// Interface untuk service layer yang mengelola logika bisnis untuk Book.
    /// </summary>
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book?> GetBookByIdAsync(int id);
        Task CreateBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task<bool> DeleteBookAsync(int id);
        Task<IEnumerable<SelectListItem>> GetPublishersForDropdownAsync();
    }
