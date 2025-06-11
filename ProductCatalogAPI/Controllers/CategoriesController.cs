using Microsoft.AspNetCore.Mvc;
using ProductCatalogAPI.DTOs;
using ProductCatalogAPI.Services;

namespace ProductCatalogAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        // GET: api/categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            
            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            return Ok(category);
        }

        // GET: api/categories/5/products
        [HttpGet("{id}/products")]
        public async Task<ActionResult<CategoryWithProductsDto>> GetCategoryWithProducts(int id)
        {
            var category = await _categoryService.GetCategoryWithProductsAsync(id);
            
            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            return Ok(category);
        }

        // POST: api/categories
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var category = await _categoryService.CreateCategoryAsync(createCategoryDto);
                return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating category: {ex.Message}");
            }
        }

        // PUT: api/categories/5
        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, UpdateCategoryDto updateCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = await _categoryService.UpdateCategoryAsync(id, updateCategoryDto);
            
            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            return Ok(category);
        }

        // DELETE: api/categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            
            if (!result)
            {
                return NotFound($"Category with ID {id} not found or has associated products.");
            }

            return NoContent();
        }
    }
}