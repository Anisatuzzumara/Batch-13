using System;
using LibraryManagementMVC.Infrastructure.Repositories;
using LibraryManagementMVC.Models;

namespace LibraryManagementMVC.Services;

public class PublisherService : IPublisherService
{
    private readonly IPublisherRepository _repo;

    public PublisherService(IPublisherRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Publisher>> GetAllPublishersAsync()
    {
        return await _repo.GetAllAsync();
    }

    public async Task<Publisher?> GetPublisherByIdAsync(int id)
    {
        return await _repo.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Publisher>> SearchPublishersAsync(string searchTerm)
    {
        var allPublishers = await _repo.GetAllAsync();
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return allPublishers;
        }
        
        return allPublishers.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
    }

    public async Task CreatePublisherAsync(Publisher publisher)
    {
        await _repo.AddAsync(publisher);
        await _repo.SaveChangesAsync();
    }

    public async Task UpdatePublisherAsync(Publisher publisher)
    {
        _repo.Update(publisher);
        await _repo.SaveChangesAsync();
    }

    public async Task<bool> DeletePublisherAsync(int id)
    {
        var publisher = await _repo.GetByIdAsync(id);
        if (publisher == null)
        {
            return false;
        }

        try
        {
            _repo.Delete(publisher);
            await _repo.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}

