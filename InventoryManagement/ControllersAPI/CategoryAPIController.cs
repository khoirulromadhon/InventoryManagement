using InventoryManagement.Service.Interface;
using InventoryManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.ControllersAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryAPI : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryAPI(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("InsertCategory")]
        public async Task<IActionResult> InsertCategory([FromBody] VmCategory vmCategory)
        {
            await _categoryService.InsertCategory(vmCategory);
            return Ok();
        }

        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories([FromQuery] string? keyword, [FromQuery] int cursor = 0)
        {
            var categories = await _categoryService.GetAllCategories(keyword, cursor);
            return Ok(categories);
        }

        [HttpGet("GetCategoryById/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            return Ok(category);
        }

        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteCategory(id);
            return Ok();
        }
    }
}
