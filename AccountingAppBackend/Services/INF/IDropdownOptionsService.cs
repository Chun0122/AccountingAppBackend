using AccountingAppBackend.Models.DTO;

namespace AccountingAppBackend.Services.INF
{
    public interface IDropdownOptionsService
    {
        Task<ApiResponse<IEnumerable<CategoryOptionDto>>> GetCategoriesAsync();
        Task<ApiResponse<IEnumerable<SubcategoryOptionDto>>> GetSubcategoriesAsync();
        Task<ApiResponse<IEnumerable<PaymentMethodOptionDto>>> GetPaymentMethodsAsync();
        Task<ApiResponse<IEnumerable<CurrencyOptionDto>>> GetCurrenciesAsync();
    }
}
