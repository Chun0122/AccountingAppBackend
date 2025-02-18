using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AccountingAppBackend.DataAccess.Models;

[Index("UserId", Name = "UserId")]
public partial class PaymentMethod
{
    [Key]
    public int PaymentMethodId { get; set; }

    [StringLength(50)]
    public string PaymentMethodName { get; set; } = null!;

    [StringLength(50)]
    public string? PaymentMethodType { get; set; }

    public uint? UserId { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("PaymentMethod")]
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    [ForeignKey("UserId")]
    [InverseProperty("PaymentMethods")]
    public virtual User? User { get; set; }
}
