using AccountingAppBackend.DataAccess.Models;
using AccountingAppBackend.Models.DTO;
using AccountingAppBackend.Models;
using AccountingAppBackend.Services.INF;
using AccountingAppBackend.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace AccountingAppBackend.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly dbContext _context;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(dbContext context, ILogger<CategoryService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ApiResponse<IEnumerable<CategoryDto>>> GetCategoriesAsync()
        {
            try
            {
                var categories = await _context.Categories
                    .Select(c => new CategoryDto
                    {
                        CategoryId = c.CategoryId.ToString(),
                        CategoryName = c.CategoryName,
                        CategoryType = c.CategoryType,
                        Description = c.Description
                    })
                    .OrderBy(c => c.CategoryType)
                    .ToListAsync();

                return new ApiResponse<IEnumerable<CategoryDto>>(true, categories, "取得帳務類別成功！");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得帳務類別失敗");
                return new ApiResponse<IEnumerable<CategoryDto>>(false, null, "取得帳務類別失敗，請稍後再試。");
            }
        }

        public async Task<ApiResponse<Category>> CreateCategoryAsync(CategoryRequest request, uint userId)
        {
            try
            {
                var categoryEntity = new Category
                {
                    CategoryName = request.CategoryName,
                    CategoryType = request.CategoryType,
                    Description = request.Description,
                    UserId = userId
                };

                _context.Categories.Add(categoryEntity);
                await _context.SaveChangesAsync();

                return new ApiResponse<Category>(true, categoryEntity, "帳務類別新增成功！");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "新增帳務類別失敗");
                return new ApiResponse<Category>(false, null, "帳務類別新增失敗，伺服器發生錯誤。");
            }
        }

        public async Task<ApiResponse<string>> UpdateCategoryAsync(int categoryId, CategoryRequest request, uint userId)
        {
            var categoryToUpdate = await _context.Categories.FindAsync(categoryId);
            if (categoryToUpdate == null)
            {
                return new ApiResponse<string>(false, null, $"找不到 Id 為 {categoryId} 的帳務類別。");
            }

            categoryToUpdate.CategoryName = request.CategoryName;
            categoryToUpdate.CategoryType = request.CategoryType;
            categoryToUpdate.Description = request.Description;
            categoryToUpdate.UserId = userId;

            try
            {
                await _context.SaveChangesAsync();
                return new ApiResponse<string>(true, null, $"Id 為 {categoryId} 的帳務類別更新成功！");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新帳務類別失敗");
                return new ApiResponse<string>(false, null, "更新帳務類別失敗，伺服器發生錯誤。");
            }
        }

        public async Task<ApiResponse<string>> DeleteCategoryAsync(int categoryId)
        {
            var categoryToDelete = await _context.Categories.FindAsync(categoryId);
            if (categoryToDelete == null)
            {
                return new ApiResponse<string>(false, null, $"找不到 Id 為 {categoryId} 的帳務類別。");
            }

            _context.Categories.Remove(categoryToDelete);

            try
            {
                await _context.SaveChangesAsync();
                return new ApiResponse<string>(true, null, $"Id 為 {categoryId} 的帳務類別刪除成功！");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刪除帳務類別失敗");
                return new ApiResponse<string>(false, null, "刪除帳務類別失敗，伺服器發生錯誤。");
            }
        }

        public async Task<ApiResponse<Category>> GetCategoryByIdAsync(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
            {
                return new ApiResponse<Category>(false, null, $"找不到 Id 為 {categoryId} 的帳務類別。");
            }
            return new ApiResponse<Category>(true, category, "取得帳務類別成功！");
        }
    }
}
