using MessagePack;

namespace VanguardCore.DataServices;

public static class Query
{
    public static List<IndexEntry>? Index(Config gConfig)
    {
        try
        {
            string indexPath = Path.Combine(gConfig.ConfigPath, "index.ivdb");
            if (!File.Exists(indexPath)) return null;
            byte[] rawIndex = File.ReadAllBytes(indexPath);
            if (rawIndex.Length == 0) return [];
            return MessagePackSerializer.Deserialize<List<IndexEntry>>(rawIndex);
        }
        catch (Exception ex)
        {
            DrawError($"ERROR: {ex.Message}", Color.Red);
            return null;
        }
    }
    public static User? LoadUserByPath(string userPath)
    {
        if (!File.Exists(userPath)) return null;
        byte[] rawUser = File.ReadAllBytes(userPath);
        return MessagePackSerializer.Deserialize<User>(rawUser);
    }
    public static User? LoadUserByFileName(string FileName, Config gConfig)
    {
        string userPath = Path.Combine(gConfig.DataBasePath, FileName);
        if (!File.Exists(userPath)) return null;
        byte[] rawUser = File.ReadAllBytes(userPath);
        return MessagePackSerializer.Deserialize<User>(rawUser);
    }
    public static User? LoadUserById(string Id, Config gConfig)
    {
        List<IndexEntry>? Index = Query.Index(gConfig);
        if (Index == null) return null;
        foreach (IndexEntry entry in Index)
        {
            if (entry.Id == Id)
            {
                string userPath = Path.Combine(gConfig.DataBasePath, entry.Path);
                if (!File.Exists(userPath)) return null;
                byte[] rawUser = File.ReadAllBytes(userPath);
                return MessagePackSerializer.Deserialize<User>(rawUser);
            }
        }
        return null;
    }
}