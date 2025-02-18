﻿using System.ComponentModel.DataAnnotations;

namespace AccountingAppBackend.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "「帳號」為必填欄位。")]
        public required string? Username { get; set; }
        [Required(ErrorMessage = "「密碼」為必填欄位。")]
        public required string? Password { get; set; }
    }
}
