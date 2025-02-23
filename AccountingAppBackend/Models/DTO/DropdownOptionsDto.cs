namespace AccountingAppBackend.Models.DTO
{
    public class CategoryOptionDto
    {
        public string Value { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
    }

    public class SubcategoryOptionDto
    {
        public string Value { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public int CategoryId { get; set; }
    }

    public class PaymentMethodOptionDto
    {
        public string Value { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
    }

    public class CurrencyOptionDto
    {
        public int Value { get; set; }
        public string Label { get; set; } = string.Empty;
    }
}
