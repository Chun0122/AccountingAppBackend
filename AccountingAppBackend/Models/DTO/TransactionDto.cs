namespace AccountingAppBackend.Models.DTO
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public int? SubcategoryId { get; set; }
        public int PaymentMethodId { get; set; }
        public int CurrencyId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string? Notes { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
