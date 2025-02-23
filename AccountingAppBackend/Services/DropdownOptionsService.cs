using AccountingAppBackend.DataAccess;
using AccountingAppBackend.Models.DTO;
using AccountingAppBackend.Services.INF;
using Microsoft.EntityFrameworkCore;

namespace AccountingAppBackend.Services
{
    public class DropdownOptionsService : IDropdownOptionsService
    {
        private readonly dbContext _context;
        private readonly ILogger<DropdownOptionsService> _logger;

        public DropdownOptionsService(dbContext context, ILogger<DropdownOptionsService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ApiResponse<IEnumerable<CategoryOptionDto>>> GetCategoriesAsync()
        {
            try
            {
                var categories = await _context.Categories
                    .Select(c => new CategoryOptionDto
                    {
                        Value = c.CategoryId.ToString(),
                        Label = c.CategoryName
                    })
                    .ToListAsync();

                return new ApiResponse<IEnumerable<CategoryOptionDto>>(true, categories, "取得帳務類別成功！");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得帳務類別失敗");
                return new ApiResponse<IEnumerable<CategoryOptionDto>>(false, null, "取得帳務類別失敗，請稍後再試。");
            }
        }

        public async Task<ApiResponse<IEnumerable<SubcategoryOptionDto>>> GetSubcategoriesAsync()
        {
            try
            {
                var subcategories = await _context.Subcategories
                    .Select(s => new SubcategoryOptionDto
                    {
                        Value = s.SubcategoryId.ToString(),
                        Label = s.SubcategoryName,
                        CategoryId = s.CategoryId
                    })
                    .ToListAsync();

                return new ApiResponse<IEnumerable<SubcategoryOptionDto>>(true, subcategories, "取得帳務子類別成功！");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得帳務子類別失敗");
                return new ApiResponse<IEnumerable<SubcategoryOptionDto>>(false, null, "取得帳務子類別失敗，請稍後再試。");
            }
        }

        public async Task<ApiResponse<IEnumerable<PaymentMethodOptionDto>>> GetPaymentMethodsAsync()
        {
            try
            {
                var paymentMethods = await _context.PaymentMethods
                    .Select(p => new PaymentMethodOptionDto
                    {
                        Value = p.PaymentMethodId.ToString(),
                        Label = p.PaymentMethodName
                    })
                    .ToListAsync();

                return new ApiResponse<IEnumerable<PaymentMethodOptionDto>>(true, paymentMethods, "取得付款方式成功！");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得付款方式失敗");
                return new ApiResponse<IEnumerable<PaymentMethodOptionDto>>(false, null, "取得付款方式失敗，請稍後再試。");
            }
        }

        public async Task<ApiResponse<IEnumerable<CurrencyOptionDto>>> GetCurrenciesAsync()
        {
            try
            {
                var currencies = await _context.Currencies
                    .Select(currency => new CurrencyOptionDto
                    {
                        Value = currency.CurrencyId,
                        Label = $"{currency.CurrencyName} ({currency.CurrencyCode})"
                    })
                    .ToListAsync();

                return new ApiResponse<IEnumerable<CurrencyOptionDto>>(true, currencies, "取得幣別成功！");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得幣別失敗");
                return new ApiResponse<IEnumerable<CurrencyOptionDto>>(false, null, "取得幣別失敗，請稍後再試。");
            }
        }
    }
}
