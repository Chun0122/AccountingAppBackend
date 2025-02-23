using AccountingAppBackend.Models.DTO;
using AccountingAppBackend.Models;
using AccountingAppBackend.Services.INF;
using AccountingAppBackend.DataAccess.Models;
using AccountingAppBackend.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace AccountingAppBackend.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly dbContext _context;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(dbContext context, ILogger<TransactionService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ApiResponse<IEnumerable<TransactionDto>>> GetTransactionsAsync(uint userId)
        {
            try
            {
                var transactions = await (from t in _context.Transactions
                                          join c in _context.Categories on t.CategoryId equals c.CategoryId
                                          where t.UserId == userId
                                          select new TransactionDto
                                          {
                                              TransactionId = t.TransactionId,
                                              TransactionDate = t.TransactionDate.ToDateTime(new TimeOnly(0, 0)),
                                              TransactionType = t.TransactionType.ToString(),
                                              CategoryId = t.CategoryId,
                                              SubcategoryId = t.SubcategoryId,
                                              PaymentMethodId = t.PaymentMethodId,
                                              CurrencyId = t.CurrencyId,
                                              Amount = t.Amount,
                                              Description = t.Description,
                                              Notes = t.Notes,
                                              CategoryName = c.CategoryName
                                          }).ToListAsync();

                return new ApiResponse<IEnumerable<TransactionDto>>(true, transactions, "交易資料取得成功！");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得交易資料失敗");
                return new ApiResponse<IEnumerable<TransactionDto>>(false, null, "取得交易資料失敗，請稍後再試。");
            }
        }

        public async Task<ApiResponse<Transaction>> CreateTransactionAsync(CreateTransactionRequest request, uint userId)
        {
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
                UserId = userId
            };

            try
            {
                _context.Transactions.Add(transactionEntity);
                await _context.SaveChangesAsync();
                return new ApiResponse<Transaction>(true, transactionEntity, "交易新增成功！");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "新增交易失敗");
                return new ApiResponse<Transaction>(false, null, "交易新增失敗，伺服器發生錯誤。");
            }
        }

        public async Task<ApiResponse<string>> UpdateTransactionAsync(int transactionId, UpdateTransactionRequest request)
        {
            var transactionToUpdate = await _context.Transactions.FindAsync(transactionId);
            if (transactionToUpdate == null)
            {
                return new ApiResponse<string>(false, null, $"找不到 Id 為 {transactionId} 的交易記錄。");
            }

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
                await _context.SaveChangesAsync();
                return new ApiResponse<string>(true, null, $"Id 為 {transactionId} 的交易記錄更新成功！");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新交易記錄失敗");
                return new ApiResponse<string>(false, null, "更新交易記錄失敗，伺服器發生錯誤。");
            }
        }

        public async Task<ApiResponse<string>> DeleteTransactionAsync(int transactionId)
        {
            var transactionToDelete = await _context.Transactions.FindAsync(transactionId);
            if (transactionToDelete == null)
            {
                return new ApiResponse<string>(false, null, $"找不到 Id 為 {transactionId} 的交易記錄。");
            }

            _context.Transactions.Remove(transactionToDelete);

            try
            {
                await _context.SaveChangesAsync();
                return new ApiResponse<string>(true, null, $"Id 為 {transactionId} 的交易記錄刪除成功！");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刪除交易記錄失敗");
                return new ApiResponse<string>(false, null, "刪除交易記錄失敗，伺服器發生錯誤。");
            }
        }

        public async Task<ApiResponse<Transaction>> GetTransactionByIdAsync(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return new ApiResponse<Transaction>(false, null, $"找不到 Id 為 {id} 的交易記錄。");
            }

            return new ApiResponse<Transaction>(true, transaction, "交易記錄取得成功！");
        }
    }
}
