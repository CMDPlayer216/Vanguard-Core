using MessagePack;

namespace VanguardCore.DataServices;

public static class Export
{
    public static ExportResult User(string Id, string destPath, Config gConfig)
    {
        if (!destPath.EndsWith(".vud", StringComparison.OrdinalIgnoreCase))
            destPath += ".vdb";
        List<IndexEntry>? Index = Query.Index(gConfig);
        if (Index == null || Index.Count == 0) return ExportResult.ThereIsNotUsersException;
        int entryIndex = Index.FindIndex((i) => i.Id == Id);
        if (entryIndex == -1) return ExportResult.UserNotFoundException;
        string path = Path.Combine(gConfig.DataBasePath, Index[entryIndex].Path);
        if (!File.Exists(path)) return ExportResult.SourceUnaccesibleException;
        try
        {
            File.Copy(path, destPath);
        }
        catch (UnauthorizedAccessException)
        {
            return ExportResult.DestinyUnaccesibleException;
        }
        catch (DirectoryNotFoundException)
        {
            return ExportResult.DestinyUnaccesibleException;
        }
        catch (FileNotFoundException)
        {
            return ExportResult.SourceUnaccesibleException;
        }
        catch (IOException)
        {
            return ExportResult.IOException;
        }
        catch
        {
            return ExportResult.DefaultException;
        }
        return ExportResult.Sucess;
    }
    public static ExportResult DataBase(string destPath, Config gConfig)
    {
        if (!destPath.EndsWith(".vdb", StringComparison.OrdinalIgnoreCase))
            destPath += ".vdb";
        List<IndexEntry>? Index = Query.Index(gConfig);
        if (Index == null || Index.Count == 0) return ExportResult.ThereIsNotUsersException;
        List<User> users = [];
        foreach (IndexEntry? entry in Index)
        {
            User? user = Query.LoadUserByFileName(entry.Path, gConfig);
            if (user == null) continue;
            users.Add(user);
        }
        if (users.Count == 0) return ExportResult.ThereIsNotUsersException;
        byte[] SerializedDatabase = MessagePackSerializer.Serialize(users);
        try
        {
            File.WriteAllBytes(destPath, SerializedDatabase);
        }
        catch (UnauthorizedAccessException)
        {
            return ExportResult.DestinyUnaccesibleException;
        }
        catch (DirectoryNotFoundException)
        {
            return ExportResult.DestinyUnaccesibleException;
        }
        catch (IOException)
        {
            return ExportResult.IOException;
        }
        catch
        {
            return ExportResult.DefaultException;
        }
        return ExportResult.Sucess;

    }
}

public enum ExportResult
{
    Sucess,
    UserNotFoundException,
    DestinyUnaccesibleException,
    DefaultException,
    SourceUnaccesibleException,
    IOException,
    ThereIsNotUsersException
}