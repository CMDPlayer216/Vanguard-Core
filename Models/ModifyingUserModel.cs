namespace VanguardCore.Models;

public class ModifyingUser
{
    public string? PrimaryRole { get; set; }
    public List<string>? AddRoles { get; set; }
    public List<string>? RemoveRoles { get; set; }
    public List<string>? AddWantedRoles { get; set; }
    public List<string>? RemoveWantedRoles { get; set; }
    required public string Id { get; set; }
    public List<string>? AddPronouns { get; set; }
    public List<string>? RemovePronouns { get; set; }
    public int? Age { get; set; }
    public DateOnly? LastStreakVerification { get; set; }
    public UserType? Type { get; set; }
    public List<UserAction>? AddPendActions { get; set; }
    public List<UserAction>? RemovePendActions { get; set; }
    public int? Streak { get; set; }
    public byte[]? AvatarImage { get; set; }
}