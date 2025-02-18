using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AccountingAppBackend.DataAccess.Models;

[Index("CategoryId", Name = "CategoryId")]
[Index("CurrencyId", Name = "CurrencyId")]
[Index("PaymentMethodId", Name = "PaymentMethodId")]
[Index("SubcategoryId", Name = "SubcategoryId")]
[Index("UserId", Name = "UserId")]
public partial class Transaction
{
    [Key]
    public int TransactionId { get; set; }

    public uint UserId { get; set; }

    public DateOnly TransactionDate { get; set; }

    [Column(TypeName = "enum('income','expense')")]
    public string TransactionType { get; set; } = null!;

    public int CategoryId { get; set; }

    public int? SubcategoryId { get; set; }

    public int PaymentMethodId { get; set; }

    public int CurrencyId { get; set; }

    [Precision(10, 2)]
    public decimal Amount { get; set; }

    [StringLength(255)]
    public string? Description { get; set; }

    [Column(TypeName = "text")]
    public string? Notes { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Transactions")]
    public virtual Category Category { get; set; } = null!;

    [ForeignKey("CurrencyId")]
    [InverseProperty("Transactions")]
    public virtual Currency Currency { get; set; } = null!;

    [ForeignKey("PaymentMethodId")]
    [InverseProperty("Transactions")]
    public virtual PaymentMethod PaymentMethod { get; set; } = null!;

    [ForeignKey("SubcategoryId")]
    [InverseProperty("Transactions")]
    public virtual Subcategory? Subcategory { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Transactions")]
    public virtual User User { get; set; } = null!;
}
