using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AccountingAppBackend.DataAccess;
using AccountingAppBackend.DataAccess.Models;
using AccountingAppBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;


namespace AccountingAppBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // 定義路由為 /api/Transactions
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly dbContext _context;

        public TransactionsController(dbContext context)
        {
            _context = context;
        }

        [HttpGet("GetTransactions")] //  GET API 路徑：/api/Transactions/GetTransactions
        public async Task<IActionResult> GetTransactions()
        {
            // 1. 從 JWT Token 中解析使用者 ID (UserID)
            var userId = Util.GetCurrentUserId(HttpContext.User);

            // 2. 使用 UserID 從資料庫查詢該使用者的交易資料
            var transactions = await (from t in _context.Transactions
                join c in _context.Categories on t.CategoryId equals c.CategoryId
                where t.UserId == userId
                select new
                {
                    t.TransactionId,
                    t.TransactionDate,
                    TransactionType = t.TransactionType.ToString(), //  枚舉轉字串
                    t.CategoryId,
                    t.SubcategoryId,
                    t.PaymentMethodId,
                    t.CurrencyId,
                    t.Amount,
                    t.Description,
                    t.Notes,
                    c.CategoryName
                }).ToListAsync();
            // 3. 返回交易資料
            return Ok(transactions); //  返回 200 OK 和交易資料 (JSON 格式)
        }

        [HttpPost("CreateTransaction")]
        public IActionResult
            CreateTransaction([FromBody] CreateTransactionRequest request) // 從 Request Body 接收 CreateTransactionRequest
        {
            //  步驟 2:  資料驗證 (Model Validation)
            if (!ModelState.IsValid)
                //  如果 Model 驗證失敗，返回 400 Bad Request，並附帶驗證錯誤訊息
                return BadRequest(ModelState);

            //  步驟 3:  將 Request Data (DTO) 轉換為 Entity Model (與之前程式碼相同)
            var transactionEntity = new Transaction
            {
                TransactionDate = DateOnly.FromDateTime(request.TransactionDate),
                TransactionType = request.TransactionType,
                CategoryId = request.CategoryId,
                SubcategoryId = request.SubcategoryId,
                PaymentMethodId = request.PaymentMethodId,
                CurrencyId = request.CurrencyId,
                Amount = request.Amount,
                Description = request.Description,
                Notes = request.Notes,
                UserId = (uint)Util.GetCurrentUserId(HttpContext.User),
            };

            //  步驟 4:  儲存交易資料到資料庫 (與之前程式碼相同)
            try
            {
                _context.Transactions.Add(transactionEntity);
                _context.SaveChanges();

                //  步驟 5:  返回 API Response (成功)
                return CreatedAtAction(nameof(GetTransaction), new { id = transactionEntity.TransactionId },
                    new //  返回 201 CreatedAtAction
                    {
                        message = "交易新增成功！",
                        transaction = transactionEntity //  將新建立的 Transaction Entity 物件一起返回
                    });
            }
            catch (Exception ex)
            {
                //  如果儲存資料庫發生例外錯誤 (與之前程式碼相同)
                Console.WriteLine($"新增交易失敗，資料庫儲存發生例外錯誤: {ex}");
                return StatusCode(500, "交易新增失敗，伺服器發生錯誤。");
            }
        }

        //  範例:  定義一個 GetTransaction Action，用於返回指定 ID 的交易記錄 (僅為示範，非必要)
        [HttpGet("{id}")] //  定義路由為 /api/Transactions/{id}，例如 /api/Transactions/123
        public IActionResult GetTransaction(int id)
        {
            var transaction = _context.Transactions.Find(id); //  根據 ID 從資料庫中查詢 Transaction Entity

            if (transaction == null)
            {
                return NotFound(); //  如果找不到，返回 404 Not Found
            }

            return Ok(transaction); //  如果找到，返回 200 OK，並包含 Transaction Entity 資料
        }
    }
}