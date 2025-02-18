using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AccountingAppBackend.DataAccess.Models;

[Index("Email", Name = "Email", IsUnique = true)]
[Index("ResetPasswordToken", Name = "ResetPasswordToken", IsUnique = true)]
[Index("Username", Name = "Username", IsUnique = true)]
[Index("VerificationToken", Name = "VerificationToken", IsUnique = true)]
public partial class User
{
    [Key]
    public uint UserId { get; set; }

    [StringLength(50)]
    public string Username { get; set; } = null!;

    [StringLength(255)]
    public string PasswordHash { get; set; } = null!;

    [StringLength(255)]
    public string PasswordSalt { get; set; } = null!;

    [StringLength(100)]
    public string? Email { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime RegistrationDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastLoginDate { get; set; }

    [Column(TypeName = "enum('Active','Inactive','Blocked')")]
    public string? UserStatus { get; set; }

    [StringLength(50)]
    public string? Role { get; set; }

    [Column("ProfileImageURL")]
    [StringLength(200)]
    public string? ProfileImageUrl { get; set; }

    [StringLength(100)]
    public string? DisplayName { get; set; }

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    public string? VerificationToken { get; set; }

    public string? ResetPasswordToken { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    [InverseProperty("User")]
    public virtual ICollection<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();

    [InverseProperty("User")]
    public virtual ICollection<Subcategory> Subcategories { get; set; } = new List<Subcategory>();

    [InverseProperty("User")]
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
