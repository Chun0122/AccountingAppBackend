using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AccountingAppBackend.DataAccess;
using AccountingAppBackend.DataAccess.Models;
using AccountingAppBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AccountingAppBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // 定義路由為 /api/Auth
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly dbContext _context;

        public AuthController(IConfiguration configuration, dbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("Login")] // 定義 Login Action 的路由為 /api/Auth/Login
        public IActionResult Login([FromBody] LoginRequest request) // 從 Request Body 接收 LoginRequest
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("帳號和密碼不能為空。"); // 返回 400 Bad Request
            }

            // 1. 根據使用者名稱，從資料庫中查詢使用者資料
            var user = _context.Users.FirstOrDefault(u => u.Username == request.Username);

            // 2. 檢查使用者是否存在
            if (user == null)
            {
                return Unauthorized("帳號或密碼錯誤。"); // 返回 401 Unauthorized，帳號不存在
            }

            // 3. 驗證密碼：使用 BCrypt.Net.BCrypt.Verify 方法，比對使用者輸入的密碼 (request.Password) 和資料庫中儲存的密碼雜湊值 (user.PasswordHash) 及 Salt 值 (user.PasswordSalt)
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized("帳號或密碼錯誤。"); // 返回 401 Unauthorized，密碼錯誤
            }
            
            var jwtToken = Util.GetToken(_configuration, user.UserId.ToString(), user.Username, user.Role);
            
            //  登入驗證成功，返回 JWT Token 和成功訊息
            return Ok(new { message = "登入成功！", token = jwtToken });
        }

        [HttpPost("Register")] // 定義 Register Action 的路由為 /api/Auth/Register
        public IActionResult Register([FromBody] RegisterRequest request) // 從 Request Body 接收 RegisterRequest
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("帳號和密碼不能為空。"); // 返回 400 Bad Request
            }

            // 檢查帳號是否已存在
            if (_context.Users.Any(u => u.Username == request.Username))
            {
                return Conflict("帳號已存在。"); // 返回 409 Conflict，表示資源衝突
            }

            //  TODO:  可以加入 Email 格式驗證、密碼複雜度驗證等額外驗證

            // 1. 產生 Salt 值
            string salt = BCrypt.Net.BCrypt.GenerateSalt();

            // 2. 使用 Salt 值雜湊密碼
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, salt);

            // 3. 建立新的 User 物件 (使用 DB-First 模式產生的 Entity 類別)
            var newUser = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                PasswordSalt = salt,
                Email = request.Email, //  Email 可能為空，如果前端沒有傳送 Email
                RegistrationDate = DateTime.UtcNow, //  使用 UTC 時間
                UserStatus = "Active", //  預設啟用狀態
                Role = "User" //  預設使用者角色
                //  TODO:  如果 RegisterRequest 中有新增其他欄位，這邊可以將值對應到 newUser
            };

            try
            {
                // 4. 將新的 User 物件加入到 DbContext 中
                _context.Users.Add(newUser);

                // 5. 將變更儲存到資料庫
                _context.SaveChanges();

                // 註冊成功
                return Ok(new { message = "註冊成功！" }); // 返回 200 OK，並包含成功訊息
            }
            catch (Exception ex)
            {
                //  註冊失敗，發生例外錯誤 (例如資料庫錯誤)
                Console.WriteLine($"註冊失敗，發生例外錯誤: {ex}"); //  記錄錯誤訊息到 Console，方便除錯
                return StatusCode(500, "註冊失敗，伺服器發生錯誤。"); // 返回 500 Internal Server Error，並包含錯誤訊息
            }
        }
    }
}