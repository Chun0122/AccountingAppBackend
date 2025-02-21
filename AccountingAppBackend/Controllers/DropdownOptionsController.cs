using AccountingAppBackend.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AccountingAppBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // 定義路由為 /api/DropdownOptions
    [Authorize]
    public class DropdownOptionsController : ControllerBase
    {
        private readonly dbContext _context;

        // 建構子，接收 dbContext 的 Dependency Injection
        public DropdownOptionsController(dbContext context)
        {
            _context = context;
        }

        [HttpGet("Categories")] // 定義路由為 /api/DropdownOptions/Categories
        public IActionResult GetCategories()
        {
            // 從資料庫中取得帳務類別資料
            var categories = _context.Categories
                .Select(c => new // 轉換成前端需要的格式
                {
                    value = c.CategoryId.ToString(), // value 屬性為字串格式的 CategoryId
                    label = c.CategoryName // label 屬性為 CategoryName
                })
                .ToList(); // 立即執行查詢並轉換成 List

            // 返回 200 OK，並將帳務類別選項資料以 JSON 格式回傳
            return Ok(categories);
        }

        [HttpGet("Subcategories")] // 定義路由為 /api/DropdownOptions/Subcategories
        public IActionResult GetSubcategories()
        {
            // 從資料庫中取得帳務子類別資料
            var subcategories = _context.Subcategories
                .Select(s => new // 轉換成前端需要的格式
                {
                    value = s.SubcategoryId.ToString(), // value 屬性為字串格式的 SubcategoryId
                    label = s.SubcategoryName, // label 屬性為 SubcategoryName
                    categoryId = s.CategoryId // category 屬性為 CategoryId
                })
                .ToList(); // 立即執行查詢並轉換成 List

            // 返回 200 OK，並將帳務子類別選項資料以 JSON 格式回傳
            return Ok(subcategories);
        }

        [HttpGet("PaymentMethods")] // 定義路由為 /api/DropdownOptions/PaymentMethods
        public IActionResult GetPaymentMethods()
        {
            // 從資料庫中取得付款方式資料
            var paymentMethods = _context.PaymentMethods
                .Select(p => new // 轉換成前端需要的格式
                {
                    value = p.PaymentMethodId.ToString(), // value 屬性為字串格式的 PaymentMethodId
                    label = p.PaymentMethodName // label 屬性為 PaymentMethodName
                })
                .ToList(); // 立即執行查詢並轉換成 List

            // 返回 200 OK，並將付款方式選項資料以 JSON 格式回傳
            return Ok(paymentMethods);
        }

        [HttpGet("Currencies")] // 定義路由為 /api/DropdownOptions/Currencies
        public IActionResult GetCurrencies()
        {
            // 從資料庫中取得幣別資料
            var currencies = _context.Currencies
                .Select(currency => new // 轉換成前端需要的格式
                {
                    value = currency.CurrencyId, // value 屬性為數字格式的 CurrencyId
                    label = $"{currency.CurrencyName} ({currency.CurrencyCode})" // label 屬性為 幣別名稱 (幣別代碼) 組合字串
                })
                .ToList(); // 立即執行查詢並轉換成 List

            // 返回 200 OK，並將幣別選項資料以 JSON 格式回傳
            return Ok(currencies);
        }
    }
}