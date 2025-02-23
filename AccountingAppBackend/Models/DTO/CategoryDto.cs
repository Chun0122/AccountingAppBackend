namespace AccountingAppBackend.Models.DTO
{
    public class CategoryDto
    {
        public string CategoryId { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string? CategoryType { get; set; }
        public string? Description { get; set; }
    }
}
