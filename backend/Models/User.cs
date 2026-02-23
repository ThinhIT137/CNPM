using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public partial class User
{
    [Key]
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Avt { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Role { get; set; }

    public DateTime? CreatedAt { get; set; }
    public string? ResetPasswordToken { get; set; } // Lưu mã 5 số
    public DateTime? ResetPasswordExpiry { get; set; }
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    [InverseProperty("User")]
    public virtual ICollection<Hottel> Hottels { get; set; }
    [InverseProperty("User")]
    public virtual ICollection<Tourist_Area> Tourist_Areas { get; set; }
}
