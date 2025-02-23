using System.ComponentModel.DataAnnotations;

namespace AccountingAppBackend.Models
{
    public class CategoryRequest
    {
        [Required(ErrorMessage = "「帳務類別名稱」為必填欄位。")]
        public string CategoryName { get; set; }
        [Required(ErrorMessage = "「帳務類別名稱」為必填欄位。")]
        public string CategoryType { get; set; }
        public string Description { get; set; }
    }
}
