using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AccountingAppBackend;

public class Util
{
    public static string GetToken(IConfiguration oConfiguration, string sUserId, string sUserName, string? sRole)
    {
        // 1. 定義 Token 的 Payload (聲明, Claims)
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, sUserId), // 使用者ID
            new Claim(ClaimTypes.Name, sUserName), // 使用者名稱
            new Claim(ClaimTypes.Role, sRole ?? "User"), // 使用者角色 (如果 Role 為 null 則預設為 "User")
        };

        // 2. 產生 Security Key (用於簽名 Token)
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(oConfiguration["JWT:KEY"] ?? "")); //  !!! 你的金鑰必須要夠長且保密，請替換成更安全的金鑰 !!!

        // 3. 定義 Token 的簽名演算法和金鑰
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        // 4. 產生 JWT Token
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims), // Payload
            Expires = DateTime.UtcNow.AddHours(1), // Token 過期時間 (例如 1 小時後過期)
            Issuer = oConfiguration["JWT:Issuer"],
            SigningCredentials = credentials, // 簽名資訊
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }
    
    //  改寫後的共用函式，接受 ClaimsPrincipal 作為參數
    public static uint? GetCurrentUserId(ClaimsPrincipal user) // 宣告為 static method，參數型別改為 ClaimsPrincipal，回傳型別改為 int? (允許 null)
    {
        if (user == null || user.Identity == null || !user.Identity.IsAuthenticated)
        {
            //  如果使用者未經驗證，或 HttpContext.User 為 null，則無法取得 UserID，返回 null
            return null; // 或您可以根據需求拋出例外，或返回預設值
        }

        //  從 ClaimsPrincipal 中，根據 ClaimTypes.NameIdentifier 取得 Claim
        var userIdClaim = user.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier) ??
                          user.Claims.FirstOrDefault(claim => claim.Type == "sub");

        if (userIdClaim != null && uint.TryParse(userIdClaim.Value, out uint userId))
        {
            //  如果找到 UserID Claim 且成功轉換為 int，則返回 UserID
            return userId;
        }

        //  如果找不到 UserID Claim 或轉換失敗，表示 Token 中沒有 UserID 資訊，或 Token 驗證有問題
        //  !!!  這裡應該要根據您的應用程式需求，決定如何處理這種情況，例如：
        //  1.  拋出例外錯誤，讓 Global Exception Handler 處理，返回 401 Unauthorized 或 500 Internal Server Error
        //  2.  返回預設的 UserID 值 (如果允許匿名使用者或系統使用者建立交易記錄)
        //  3.  記錄警告訊息，並返回錯誤回應
        //  為了共用函式的彈性，這裡建議返回 null，讓調用者決定如何處理
        return null; //  返回 null 表示無法取得 UserID
    }
}