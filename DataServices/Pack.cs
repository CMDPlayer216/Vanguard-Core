using MessagePack;
namespace VanguardCore.DataServices;

public static class Pack
{
    public static List<User>? UnpackDataBase(string path)
    {
        if (!File.Exists(path)) return null;
        byte[] rawData = File.ReadAllBytes(path);
        if (rawData.Length == 0) return null;
        return MessagePackSerializer.Deserialize<List<User>>(rawData);
    }
}
