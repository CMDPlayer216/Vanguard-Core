using MessagePack;
using NanoidDotNet;
using System.Text.Json.Serialization; // Directiva agregada

namespace VanguardCore.Models;

[MessagePackObject]
public class User
{
    [Key(0)]
    [JsonPropertyName("primaryRole")]
    required public string PrimaryRole { get; set; }

    [Key(1)]
    [JsonPropertyName("roles")]
    public List<string>? Roles { get; set; }

    [Key(2)]
    [JsonPropertyName("wantedRoles")]
    public List<string>? WantedRoles { get; set; }

    [Key(3)]
    [JsonPropertyName("id")]
    public string Id { get; set; } = Nanoid.Generate(size: 12);

    [Key(4)]
    [JsonPropertyName("version")]
    public int Version { get; set; } = 1;

    [Key(5)]
    [JsonPropertyName("pronouns")]
    required public List<string> Pronouns { get; set; }

    [Key(6)]
    [JsonPropertyName("age")]
    required public int Age { get; set; }

    [Key(7)]
    [JsonPropertyName("creationTime")]
    public DateTime CreationTime { get; set; } = DateTime.Now;

    [Key(8)]
    [JsonPropertyName("lastStreakVerification")]
    public DateOnly? LastStreakVerification { get; set; }

    [Key(9)]
    [JsonPropertyName("type")]
    public UserType Type { get; set; } = UserType.Member;

    [Key(10)]
    [JsonPropertyName("pendActions")]
    public List<UserAction>? PendActions { get; set; }

    [Key(11)]
    [JsonPropertyName("streak")]
    public int Streak { get; set; } = 0;

    // Se serializará como una cadena Base64 automáticamente en JSON
    [Key(12)]
    [JsonIgnore] // Ignora el byte[] nativo en la serialización JSON
    public byte[]? AvatarImage { get; set; }

    // Propiedad auxiliar que convierte a Base64 para JSON
    [IgnoreMember] // Para que MessagePack la ignore
    [JsonPropertyName("avatarImageBase64")]
    public string? AvatarImageBase64
    {
        get => AvatarImage != null ? Convert.ToBase64String(AvatarImage) : null;
        set => AvatarImage = value != null ? Convert.FromBase64String(value) : null;
    }

    [Key(13)]
    [JsonPropertyName("fandoms")]
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