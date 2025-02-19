using System;
using System.ComponentModel.DataAnnotations;

namespace AccountingAppBackend.Models
{
    public class UpdateTransactionRequest
    {
        [Required(ErrorMessage = "「交易日期」為必填欄位。")]
        public DateTime TransactionDate { get; set; } // 交易日期

        [Required(ErrorMessage = "「交易類別」為必填欄位。")]
        public string TransactionType { get; set; } // 交易類別 (收入/支出)

        [Required(ErrorMessage = "「帳務類別」為必填欄位。")]
        public int CategoryId { get; set; } // 帳務類別 Id

        public int? SubcategoryId { get; set; } // 帳務子類別 Id (可選)

        [Required(ErrorMessage = "「付款方式」為必填欄位。")]
        public int PaymentMethodId { get; set; } // 付款方式 Id

        public int CurrencyId { get; set; } // 幣別 Id

        [Required(ErrorMessage = "「金額」為必填欄位。")]
        [Range(0.01, double.MaxValue, ErrorMessage = "「金額」必須大於 0。")] // 金額需大於 0
        public decimal Amount { get; set; } // 金額

        [MaxLength(50, ErrorMessage = "「描述」長度請勿超過 50 個字元。")]
        public string Description { get; set; } // 描述 (可選)

        [MaxLength(200, ErrorMessage = "「備註」長度請勿超過 200 個字元。")]
        public string Notes { get; set; } // 備註 (可選)
    }
}