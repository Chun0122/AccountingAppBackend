using AccountingAppBackend.Models.DTO;
using AccountingAppBackend.Models;
using AccountingAppBackend.DataAccess.Models;

namespace AccountingAppBackend.Services.INF
{
    public interface ITransactionService
    {
        Task<ApiResponse<IEnumerable<TransactionDto>>> GetTransactionsAsync(uint userId);
        Task<ApiResponse<Transaction>> CreateTransactionAsync(CreateTransactionRequest request, uint userId);
        Task<ApiResponse<string>> UpdateTransactionAsync(int transactionId, UpdateTransactionRequest request);
        Task<ApiResponse<string>> DeleteTransactionAsync(int transactionId);
        Task<ApiResponse<Transaction>> GetTransactionByIdAsync(int id);
    }
}
