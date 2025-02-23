using AccountingAppBackend.Models;

namespace AccountingAppBackend.Services.INF
{
    public interface IAuthService
    {
        Task<(bool Success, string? Token, string Message)> LoginAsync(LoginRequest request);
        Task<(bool Success, string Message)> RegisterAsync(RegisterRequest request);
    }
}
