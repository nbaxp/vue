public class User : BaseEntity
{
    public string UserName { get; set; } = null!;

    /// <summary>
    /// 大写形式，唯一，避免只有大小写不同
    /// </summary>
    public string? NormalizedUserName { get; set; }

    public string? SecurityStamp { get; set; }
    public string? PasswordHash { get; set; }

    public string? Email { get; set; }

    /// <summary>
    /// 大写形式，唯一，避免只有大小写不同
    /// </summary>
    public string? NormalizedEmail { get; set; }

    public bool EmailConfirmed { get; set; }
    public string? PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public bool LockoutEnabled { get; set; }
    public DateTime? LockoutEnd { get; set; }
    public int AccessFailedCount { get; set; }
    public bool TwoFactorEnabled { get; set; }
    /// <summary>
    /// 并发标记
    /// </summary>
    //public string? ConcurrencyStamp { get; set; }
}
