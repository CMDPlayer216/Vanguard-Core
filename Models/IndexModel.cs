using System.Text.Json.Serialization;
using MessagePack;
using NanoidDotNet;
namespace VanguardCore.Models;

[MessagePackObject]
public class IndexEntry
{
    [Key(0)]
    [JsonPropertyName("primaryRole")]
    public string PrimaryRole { get; set; } = "Unknown";
    [Key(1)]
    [JsonPropertyName("id")]
    public string Id { get; set; } = Nanoid.Generate(size: 12);
    [Key(2)]
    [JsonPropertyName("version")]
    public int Version { get; set; } = 1;
    [Key(3)]
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
    [Key(4)]
    [JsonPropertyName("path")]
    public string Path { get; set; } = $"{Guid.NewGuid()}.vud";
}