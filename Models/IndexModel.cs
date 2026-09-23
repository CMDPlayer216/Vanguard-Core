using MessagePack;
using NanoidDotNet;
namespace VanguardCore.Models;

[MessagePackObject]
public class IndexEntry
{
    [Key(0)]
    public string PrimaryRole { get; set; } = "Unknown";
    [Key(1)]
    public string Id { get; set; } = Nanoid.Generate(size: 12);
    [Key(2)]
    public int Version { get; set; } = 1;
    [Key(3)]
    public byte[]? AvatarImage { get; set; }
    [Key(4)]
    public string Path { get; set; } = $"{Guid.NewGuid()}.vud";
}