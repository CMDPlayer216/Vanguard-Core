using MessagePack;

namespace VanguardCore.Models;

#pragma warning disable RCS1102
[MessagePackObject]
public record class UserAction : IEquatable<UserAction>
{
    [Key(0)]
    public DateTime ActionTimeTrigger { get; set; }
    [Key(1)]
    public Action ActionType { get; set; }
    [Key(2)]
    public string? Reason { get; set; }
}
#pragma warning restore RCS1102

public enum Action
{
    Ban,
    Unban,
    Kick
}