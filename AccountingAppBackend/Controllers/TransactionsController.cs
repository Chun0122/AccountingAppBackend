using AccountingAppBackend.DataAccess;
using AccountingAppBackend.DataAccess.Models;
using AccountingAppBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


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

        [HttpPut("{transactionId}")] // 定義路由為 /api/Transactions/UpdateTransaction/{transactionId}，HTTP 方法為 PUT
        public async Task<IActionResult> UpdateTransaction(int transactionId, [FromBody] UpdateTransactionRequest request) // 從 Request Body 接收 UpdateTransactionRequest
        {
            if (!ModelState.IsValid)
            {
                // 如果 Model 驗證失敗，返回 400 Bad Request，並附帶驗證錯誤訊息
                return BadRequest(ModelState);
            }

            // 1. 根據 transactionId，從資料庫中查詢要更新的交易記錄
            var transactionToUpdate = await _context.Transactions.FindAsync(transactionId);

            // 2. 檢查交易記錄是否存在
            if (transactionToUpdate == null)
            {
                // 如果找不到對應的交易記錄，返回 404 Not Found
                return NotFound($"找不到 Id 為 {transactionId} 的交易記錄。");
            }

            // 3. 更新交易記錄的屬性 (將 Request Body 中的值，對應到資料庫中的 Entity)
            transactionToUpdate.TransactionDate = DateOnly.FromDateTime(request.TransactionDate);
            transactionToUpdate.TransactionType = request.TransactionType;
            transactionToUpdate.CategoryId = request.CategoryId;
            transactionToUpdate.SubcategoryId = request.SubcategoryId;
            transactionToUpdate.PaymentMethodId = request.PaymentMethodId;
            transactionToUpdate.CurrencyId = request.CurrencyId;
            transactionToUpdate.Amount = request.Amount;
            transactionToUpdate.Description = request.Description;
            transactionToUpdate.Notes = request.Notes;

            try
            {
                // 4. 將變更儲存到資料庫
                await _context.SaveChangesAsync();

                // 更新成功，返回 200 OK，並包含成功訊息
                return Ok(new { message = $"Id 為 {transactionId} 的交易記錄更新成功！" });
            }
            catch (Exception ex)
            {
                // 更新失敗，發生例外錯誤 (例如資料庫錯誤)
                Console.WriteLine($"更新交易記錄失敗，發生例外錯誤: {ex}"); // 記錄錯誤訊息到 Console，方便除錯
                return StatusCode(500, "更新交易記錄失敗，伺服器發生錯誤。"); // 返回 500 Internal Server Error，並包含錯誤訊息
            }
        }

        //  定義一個 GetTransaction Action，用於返回指定 ID 的交易記錄 (僅為示範，非必要)
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