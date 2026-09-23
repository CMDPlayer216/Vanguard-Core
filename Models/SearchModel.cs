namespace VanguardCore.Models;

public class SearchFilters
{
    public string? PrimaryRole { get; set; }
    public string? Id { get; set; }
    public string? Rol { get; set; }
    public string? WantedRol { get; set; }
    public string? Pronoun { get; set; }
    public int? Age { get; set; }

    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
    public DateTime? CreationTime { get; set; }
    public DateTime? MinCreationTime { get; set; }
    public DateTime? MaxCreationTime { get; set; }
    public DateOnly? LastStreakVerification { get; set; }
    public DateOnly? MinLastStreakVerification { get; set; }
    public DateOnly? MaxLastStreakVerification { get; set; }
    public UserType? Type { get; set; }
    public int? Streak { get; set; }
    public int? MinStreak { get; set; }
    public int? MaxStreak { get; set; }
    public string? Fandom { get; set; }
    public bool FastSearch { get; set; }
}