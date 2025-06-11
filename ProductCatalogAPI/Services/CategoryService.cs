using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Data;
using ProductCatalogAPI.DTOs;
using ProductCatalogAPI.Models;

namespace ProductCatalogAPI.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int id);
        Task<CategoryWithProductsDto?> GetCategoryWithProductsAsync(int id);
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto);
        Task<CategoryDto?> UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto);
        Task<bool> DeleteCategoryAsync(int id);
    }

    public class CategoryService : ICategoryService
    {
        private readonly ProductCatalogDbContext _context;

        public CategoryService(ProductCatalogDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .Where(c => c.IsActive)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    CreatedDate = c.CreatedDate,
                    IsActive = c.IsActive
                })
                .ToListAsync();
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories
                .Where(c => c.Id == id && c.IsActive)
                .FirstOrDefaultAsync();

            if (category == null) return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedDate = category.CreatedDate,
                IsActive = category.IsActive
            };
        }

        public async Task<CategoryWithProductsDto?> GetCategoryWithProductsAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products.Where(p => p.IsActive))
                .Where(c => c.Id == id && c.IsActive)
                .FirstOrDefaultAsync();

            if (category == null) return null;

            return new CategoryWithProductsDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedDate = category.CreatedDate,
                IsActive = category.IsActive,
                Products = category.Products.Select(p => new ProductSummaryDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CategoryName = category.Name,
                    IsInStock = p.IsInStock()
                }).ToList()
            };
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            var category = new Category
            {
                Name = createCategoryDto.Name,
                Description = createCategoryDto.Description,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedDate = category.CreatedDate,
                IsActive = category.IsActive
            };
        }

        public async Task<CategoryDto?> UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null || !category.IsActive) return null;

            category.Name = updateCategoryDto.Name;
            category.Description = updateCategoryDto.Description;
            category.IsActive = updateCategoryDto.IsActive;

            await _context.SaveChangesAsync();

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedDate = category.CreatedDate,
                IsActive = category.IsActive
            };
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            // Check if category has products
            var hasProducts = await _context.Products.AnyAsync(p => p.CategoryId == id && p.IsActive);
            if (hasProducts) return false;

            category.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}