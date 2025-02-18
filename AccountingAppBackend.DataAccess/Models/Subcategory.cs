using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AccountingAppBackend.DataAccess.Models;

[Index("CategoryId", Name = "CategoryId")]
[Index("UserId", Name = "UserId")]
public partial class Subcategory
{
    [Key]
    public int SubcategoryId { get; set; }

    [StringLength(50)]
    public string SubcategoryName { get; set; } = null!;

    public int CategoryId { get; set; }

    public uint? UserId { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Subcategories")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("Subcategory")]
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    [ForeignKey("UserId")]
    [InverseProperty("Subcategories")]
    public virtual User? User { get; set; }
}
