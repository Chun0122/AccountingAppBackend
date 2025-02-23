using AccountingAppBackend.DataAccess;
using AccountingAppBackend.DataAccess.Models;
using AccountingAppBackend.Models;
using AccountingAppBackend.Services.INF;
using Microsoft.EntityFrameworkCore;

namespace AccountingAppBackend.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly dbContext _context;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IConfiguration configuration, dbContext context, ILogger<AuthService> logger)
        {
            _configuration = configuration;
            _context = context;
            _logger = logger;
        }

        public async Task<(bool Success, string? Token, string Message)> LoginAsync(LoginRequest request)
        {
            // 根據使用者名稱查詢使用者資料 (非同步方式)
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null)
            {
                return (false, null, "帳號或密碼錯誤。");
            }

            // 驗證密碼
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return (false, null, "帳號或密碼錯誤。");
            }

            // 使用工具方法產生 JWT Token
            var token = Util.GetToken(_configuration, user.UserId.ToString(), user.Username, user.Role);
            return (true, token, "登入成功！");
        }

        public async Task<(bool Success, string Message)> RegisterAsync(RegisterRequest request)
        {
            // 檢查帳號是否已存在
            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            {
                return (false, "帳號已存在。");
            }

            // 產生 Salt 並雜湊密碼
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, salt);

            // 建立新的使用者物件
            var newUser = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                PasswordSalt = salt,
                Email = request.Email,
                RegistrationDate = DateTime.UtcNow,
                UserStatus = "Active",
                Role = "User"
            };

            try
            {
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
                return (true, "註冊成功！");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "註冊失敗");
                return (false, "註冊失敗，伺服器發生錯誤。");
            }
        }
    }
}
