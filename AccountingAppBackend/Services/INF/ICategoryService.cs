using AccountingAppBackend.DataAccess.Models;
using AccountingAppBackend.Models.DTO;
using AccountingAppBackend.Models;

namespace AccountingAppBackend.Services.INF
{
    public interface ICategoryService
    {
        Task<ApiResponse<IEnumerable<CategoryDto>>> GetCategoriesAsync();
        Task<ApiResponse<Category>> CreateCategoryAsync(CategoryRequest request, uint userId);
        Task<ApiResponse<string>> UpdateCategoryAsync(int categoryId, CategoryRequest request, uint userId);
        Task<ApiResponse<string>> DeleteCategoryAsync(int categoryId);
        Task<ApiResponse<Category>> GetCategoryByIdAsync(int categoryId);
    }
}
