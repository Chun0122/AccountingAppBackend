using AccountingAppBackend.DataAccess;
using AccountingAppBackend.DataAccess.Models;
using AccountingAppBackend.Models;
using AccountingAppBackend.Models.DTO;
using AccountingAppBackend.Services.INF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountingAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesListController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesListController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            var response = await _categoryService.GetCategoriesAsync();
            if (!response.Success)
            {
                return StatusCode(500, response);
            }
            return Ok(response);
        }

        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<Category>(false, null, "資料驗證失敗"));
            }

            uint userId = (uint)Util.GetCurrentUserId(HttpContext.User);
            var response = await _categoryService.CreateCategoryAsync(request, userId);
            if (!response.Success)
            {
                return StatusCode(500, response);
            }

            return CreatedAtAction(nameof(GetCategory), new { id = response.Data?.CategoryId }, response);
        }

        [HttpPut("UpdateCategory/{categoryId}")]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] CategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<string>(false, null, "資料驗證失敗"));
            }

            uint userId = (uint)Util.GetCurrentUserId(HttpContext.User);
            var response = await _categoryService.UpdateCategoryAsync(categoryId, request, userId);
            if (!response.Success)
            {
                if (response.Message.Contains("找不到"))
                    return NotFound(response);
                return StatusCode(500, response);
            }

            return Ok(response);
        }

        [HttpDelete("DeleteCategory/{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var response = await _categoryService.DeleteCategoryAsync(categoryId);
            if (!response.Success)
            {
                if (response.Message.Contains("找不到"))
                    return NotFound(response);
                return StatusCode(500, response);
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var response = await _categoryService.GetCategoryByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
