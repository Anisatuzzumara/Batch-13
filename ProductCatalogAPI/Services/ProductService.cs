using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Data;
using ProductCatalogAPI.DTOs;
using ProductCatalogAPI.Models;

namespace ProductCatalogAPI.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductSummaryDto>> GetAllProductsAsync(int pageNumber = 1, int pageSize = 10);
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductSummaryDto>> SearchProductsAsync(string query);
        Task<IEnumerable<ProductSummaryDto>> GetProductsByCategoryAsync(int categoryId);
        Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);
        Task<ProductDto?> UpdateProductAsync(int id, UpdateProductDto updateProductDto);
        Task<bool> DeleteProductAsync(int id);
    }

    public class ProductService : IProductService
    {
        private readonly ProductCatalogDbContext _context;

        public ProductService(ProductCatalogDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductSummaryDto>> GetAllProductsAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductSummaryDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CategoryName = p.Category.Name,
                    IsInStock = p.StockQuantity > 0
                })
                .ToListAsync();
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Id == id && p.IsActive)
                .FirstOrDefaultAsync();

            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name,
                CreatedDate = product.CreatedDate,
                LastModifiedDate = product.LastModifiedDate,
                IsActive = product.IsActive,
                IsInStock = product.IsInStock(),
                IsLowStock = product.IsLowStock()
            };
        }

        public async Task<IEnumerable<ProductSummaryDto>> SearchProductsAsync(string query)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive && 
                           (p.Name.Contains(query) || 
                            (p.Description != null && p.Description.Contains(query))))
                .Select(p => new ProductSummaryDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CategoryName = p.Category.Name,
                    IsInStock = p.StockQuantity > 0
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductSummaryDto>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId && p.IsActive)
                .Select(p => new ProductSummaryDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CategoryName = p.Category.Name,
                    IsInStock = p.StockQuantity > 0
                })
                .ToListAsync();
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
        {
            var product = new Product
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                StockQuantity = createProductDto.StockQuantity,
                CategoryId = createProductDto.CategoryId,
                CreatedDate = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow,
                IsActive = true
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Load category for response
            await _context.Entry(product).Reference(p => p.Category).LoadAsync();

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name,
                CreatedDate = product.CreatedDate,
                LastModifiedDate = product.LastModifiedDate,
                IsActive = product.IsActive,
                IsInStock = product.IsInStock(),
                IsLowStock = product.IsLowStock()
            };
        }

        public async Task<ProductDto?> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null || !product.IsActive) return null;

            product.Name = updateProductDto.Name;
            product.Description = updateProductDto.Description;
            product.Price = updateProductDto.Price;
            product.StockQuantity = updateProductDto.StockQuantity;
            product.CategoryId = updateProductDto.CategoryId;
            product.IsActive = updateProductDto.IsActive;
            product.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Reload category if changed
            await _context.Entry(product).Reference(p => p.Category).LoadAsync();

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name,
                CreatedDate = product.CreatedDate,
                LastModifiedDate = product.LastModifiedDate,
                IsActive = product.IsActive,
                IsInStock = product.IsInStock(),
                IsLowStock = product.IsLowStock()
            };
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            // Soft delete
            product.IsActive = false;
            product.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}