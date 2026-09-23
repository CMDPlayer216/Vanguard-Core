using MessagePack;
using NanoidDotNet;
namespace VanguardCore.Models;

[MessagePackObject]
public class User
{
    [Key(0)]
    required public string PrimaryRole { get; set; }
    [Key(1)]
    public List<string>? Roles { get; set; }
    [Key(2)]
    public List<string>? WantedRoles { get; set; }
    [Key(3)]
    public string Id { get; set; } = Nanoid.Generate(size: 12);
    [Key(4)]
    public int Version { get; set; } = 1;
    [Key(5)]
    required public List<string> Pronouns { get; set; }
    [Key(6)]
    required public int Age { get; set; }
    [Key(7)]
    public DateTime CreationTime { get; set; } = DateTime.Now;
    [Key(8)]
    public DateOnly? LastStreakVerification { get; set; }
    [Key(9)]
    public UserType Type { get; set; } = UserType.Member;
    [Key(10)]
    public List<UserAction>? PendActions { get; set; }
    [Key(11)]
    public int Streak { get; set; } = 0;
    [Key(12)]
    public byte[]? AvatarImage { get; set; }
    [Key(13)]
    required public List<string> Fandoms { get; set; }
}

public enum UserType
{
    Administrator,
    Helper,
    Member
}

public enum UserStatus
{
    Active,
    Inactive,
    Banned,
    Kicked
}