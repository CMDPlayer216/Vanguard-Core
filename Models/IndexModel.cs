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
    [JsonPropertyName("path")]
    public string Path { get; set; } = $"{Guid.NewGuid()}.vud";
}