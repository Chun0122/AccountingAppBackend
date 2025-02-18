using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AccountingAppBackend.DataAccess.Models;

[Index("CurrencyCode", Name = "CurrencyCode", IsUnique = true)]
public partial class Currency
{
    [Key]
    public int CurrencyId { get; set; }

    [StringLength(10)]
    public string CurrencyCode { get; set; } = null!;

    [StringLength(50)]
    public string? CurrencyName { get; set; }

    [StringLength(10)]
    public string? CurrencySymbol { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("Currency")]
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
