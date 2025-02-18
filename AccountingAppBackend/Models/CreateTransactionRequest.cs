using System.ComponentModel.DataAnnotations;

namespace AccountingAppBackend.Models
{
    public class CreateTransactionRequest
    {
        [Required(ErrorMessage = "「交易日期」為必填欄位。")]
        public DateTime TransactionDate { get; set; }

        [Required(ErrorMessage = "「交易類別」為必填欄位。")]
        public required string TransactionType { get; set; }

        [Required(ErrorMessage = "「帳務類別」為必填欄位。")]
        [Range(1, int.MaxValue, ErrorMessage = "「帳務類別」為必填欄位。")]
        public int CategoryId { get; set; }
        
        public int? SubcategoryId { get; set; }

        [Required(ErrorMessage = "「付款方式」為必填欄位。")]
        [Range(1, int.MaxValue, ErrorMessage = "「付款方式」為必填欄位。")]
        public int PaymentMethodId { get; set; }

        [Required(ErrorMessage = "「幣別」為必填欄位。")]
        [Range(1, int.MaxValue, ErrorMessage = "「幣別」為必填欄位。")]
        public int CurrencyId { get; set; }

        [Required(ErrorMessage = "「金額」為必填欄位。")]
        [Range(0.01, double.MaxValue, ErrorMessage = "「金額」必須大於 0。")]
        public decimal Amount { get; set; }

        [MaxLength(50, ErrorMessage = "「描述」長度請勿超過 50 個字元。")]
        public string? Description { get; set; }

        [MaxLength(200, ErrorMessage = "「備註」長度請勿超過 200 個字元。")]
        public string? Notes { get; set; }
    }
}