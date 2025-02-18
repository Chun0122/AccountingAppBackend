using System.ComponentModel.DataAnnotations;

namespace AccountingAppBackend.Models
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "「帳號」為必填欄位。")]
        public required string Username { get; set; }
        [Required(ErrorMessage = "「密碼」為必填欄位。")]
        public required string Password { get; set; }
        [MaxLength(100, ErrorMessage = "「Email」長度請勿超過 50 個字元。")]
        public string? Email { get; set; }
    }
}
